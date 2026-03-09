using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#if NET8_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#else
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
#endif

namespace NOrganizze
{
    /// <summary>
    /// Strongly-typed .NET client for the Organizze HTTP API. Create with <see cref="HttpClient"/> and credentials (or email + API key).
    /// Use <see cref="BaseUrl"/> to override the API endpoint when needed (e.g. for testing).
    /// </summary>
    public class NOrganizzeClient : IDisposable
    {
        /// <summary>Default base URL for the official Organizze REST v2 API. Can be overridden via constructor parameters.</summary>
        [SuppressMessage("SonarQube", "S1075:URIs should not be hardcoded",
            Justification = "This is a configurable default value for the official Organizze API endpoint. Users can override it via constructor parameters.")]
        public const string OrganizzeRestV2Url = "https://api.organizze.com.br/rest/v2";

        private readonly HttpClient _httpClient;
        private readonly bool _disposeHttpClient;
#if NET8_0_OR_GREATER
        private readonly JsonSerializerOptions _jsonOptions;
#else
        private readonly JsonSerializerSettings _jsonSettings;
#endif

        /// <summary>Provider used to obtain credentials (email + API key) for each request. Set by the constructor.</summary>
        public Func<Credentials> CredentialsProvider { get; }
        /// <summary>Base URL for Organizze API requests. Defaults to <see cref="OrganizzeRestV2Url"/> unless overridden in the constructor.</summary>
        public string BaseUrl { get; }

        /// <summary>Access user-related API operations.</summary>
        public Users.UserService Users { get; }
        /// <summary>List, create, update, and delete bank accounts.</summary>
        public Accounts.AccountService Accounts { get; }
        /// <summary>Manage categories.</summary>
        public Categories.CategoryService Categories { get; }
        /// <summary>Manage credit cards.</summary>
        public CreditCards.CreditCardService CreditCards { get; }
        /// <summary>List and inspect credit card invoices and their payment transaction.</summary>
        public Invoices.InvoiceService Invoices { get; }
        /// <summary>List, create, update, and delete transactions (expenses, incomes). Use <see cref="Transactions.TransactionListOptions"/> to filter by date and account.</summary>
        public Transactions.TransactionService Transactions { get; }
        /// <summary>Manage transfers between accounts.</summary>
        public Transfers.TransferService Transfers { get; }
        /// <summary>List budgets by year or month.</summary>
        public Budgets.BudgetService Budgets { get; }

        /// <summary>
        /// Creates a client using an existing <see cref="HttpClient"/> and a credentials provider. Use this when you manage <see cref="HttpClient"/> yourself (e.g. in ASP.NET Core).
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests. Not disposed by this client.</param>
        /// <param name="credentialsProvider">Function that returns the credentials (email + API key) to use for authentication.</param>
        /// <param name="baseUrl">Optional. Base URL for the API. Defaults to <see cref="OrganizzeRestV2Url"/>.</param>
        public NOrganizzeClient(HttpClient httpClient, Func<Credentials> credentialsProvider, string baseUrl = OrganizzeRestV2Url)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _disposeHttpClient = false;

            CredentialsProvider = credentialsProvider ?? throw new ArgumentNullException(nameof(credentialsProvider));
            BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));

#if NET8_0_OR_GREATER
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
#else
            _jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
#endif

            ConfigureHttpClient();

            Users = new Users.UserService(this);
            Accounts = new Accounts.AccountService(this);
            Categories = new Categories.CategoryService(this);
            CreditCards = new CreditCards.CreditCardService(this);
            Invoices = new Invoices.InvoiceService(this);
            Transactions = new Transactions.TransactionService(this);
            Transfers = new Transfers.TransferService(this);
            Budgets = new Budgets.BudgetService(this);
        }

        /// <summary>
        /// Creates a client using an existing <see cref="HttpClient"/> with email and API key. The client does not dispose the HTTP client.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests.</param>
        /// <param name="email">Organizze account email.</param>
        /// <param name="apiKey">Organizze API key.</param>
        /// <param name="baseUrl">Optional. Base URL for the API. Defaults to <see cref="OrganizzeRestV2Url"/>.</param>
        public NOrganizzeClient(HttpClient httpClient, string email, string apiKey, string baseUrl = OrganizzeRestV2Url)
            : this(httpClient, () => new Credentials(email, apiKey), baseUrl)
        {
        }

        /// <summary>
        /// Creates a client with a credentials provider; the client creates and owns an internal <see cref="HttpClient"/> and will dispose it when <see cref="Dispose()"/> is called.
        /// </summary>
        /// <param name="credentialsProvider">Function that returns the credentials to use for authentication.</param>
        /// <param name="baseUrl">Optional. Base URL for the API. Defaults to <see cref="OrganizzeRestV2Url"/>.</param>
        public NOrganizzeClient(Func<Credentials> credentialsProvider, string baseUrl = OrganizzeRestV2Url)
            : this(new HttpClient(), credentialsProvider, baseUrl)
        {
            _disposeHttpClient = true;
        }

        /// <summary>
        /// Creates a client with email and API key; the client creates and owns an internal <see cref="HttpClient"/> and will dispose it when <see cref="Dispose()"/> is called.
        /// </summary>
        /// <param name="email">Organizze account email.</param>
        /// <param name="apiKey">Organizze API key.</param>
        /// <param name="baseUrl">Optional. Base URL for the API. Defaults to <see cref="OrganizzeRestV2Url"/>.</param>
        public NOrganizzeClient(string email, string apiKey, string baseUrl = OrganizzeRestV2Url)
            : this(() => new Credentials(email, apiKey), baseUrl)
        {
        }

        private void ConfigureHttpClient()
        {
            var credentials = CredentialsProvider();

            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(credentials.ToUserAgentHeaderValue());

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials.ToBasicAuthHeaderValue());
        }

        internal T Request<T>(
            HttpMethod method,
            string path,
            object content = null,
            RequestOptions requestOptions = null)
        {
            var request = BuildRequest(method, path, content, requestOptions);

#if NET8_0_OR_GREATER
            using var response = _httpClient.Send(request);
#else
            using var response = _httpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
#endif

            return HandleResponseAsync<T>(response, CancellationToken.None).GetAwaiter().GetResult();
        }

        internal void Request(
            HttpMethod method,
            string path,
            object content = null,
            RequestOptions requestOptions = null)
        {
            var request = BuildRequest(method, path, content, requestOptions);

#if NET8_0_OR_GREATER
            using var response = _httpClient.Send(request);
#else
            using var response = _httpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
#endif

            EnsureSuccessAsync(response, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        internal async Task<T> RequestAsync<T>(
            HttpMethod method,
            string path,
            object content = null,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            var request = BuildRequest(method, path, content, requestOptions);

            using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            return await HandleResponseAsync<T>(response, cancellationToken).ConfigureAwait(false);
        }

        internal async Task RequestAsync(
            HttpMethod method,
            string path,
            object content = null,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            var request = BuildRequest(method, path, content, requestOptions);

            using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);
        }

        private HttpRequestMessage BuildRequest(
            HttpMethod method,
            string path,
            object content,
            RequestOptions requestOptions)
        {
            var uri = path.StartsWith("http") ? path : $"{BaseUrl.TrimEnd('/')}/{path.TrimStart('/')}";

            // Override base URL if specified
            if (!string.IsNullOrEmpty(requestOptions?.BaseUrl))
                uri = $"{requestOptions.BaseUrl.TrimEnd('/')}/{path.TrimStart('/')}";

            var request = new HttpRequestMessage(method, uri);

            // Override auth if specified
            if (requestOptions?.CredentialsProvider != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", requestOptions.CredentialsProvider().ToBasicAuthHeaderValue());

            // Override user agent if specified
            if (!string.IsNullOrEmpty(requestOptions?.UserAgent))
            {
                request.Headers.UserAgent.Clear();
                request.Headers.UserAgent.ParseAdd(requestOptions.UserAgent);
            }

            // Add content if present
            if (content != null)
            {
#if NET8_0_OR_GREATER
                var json = JsonSerializer.Serialize(content, _jsonOptions);
#else
                var json = JsonConvert.SerializeObject(content, _jsonSettings);
#endif
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return request;
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            await EnsureSuccessAsync(response, cancellationToken).ConfigureAwait(false);

#if NET8_0_OR_GREATER
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

            if (string.IsNullOrWhiteSpace(responseContent))
                return default;

#if NET8_0_OR_GREATER
            return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
#else
            return JsonConvert.DeserializeObject<T>(responseContent, _jsonSettings);
#endif
        }

        private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
                await ThrowApiExceptionAsync(response, cancellationToken).ConfigureAwait(false);
        }

        private static async Task ThrowApiExceptionAsync(HttpResponseMessage response, CancellationToken cancellationToken)
        {
#if NET8_0_OR_GREATER
            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

            throw new NOrganizzeException(
                $"API request failed with status {(int)response.StatusCode}: {response.ReasonPhrase}",
                response.StatusCode,
                content);
        }

        /// <summary>Releases resources. If the client created its own <see cref="HttpClient"/>, that client is disposed.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Releases managed resources. Override in derived classes to add custom disposal logic.</summary>
        /// <param name="disposing">True when called from <see cref="Dispose()"/>; false when called from the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_disposeHttpClient)
            {
                _httpClient?.Dispose();
            }
        }
    }
}
