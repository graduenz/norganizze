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
    public class NOrganizzeClient : IDisposable
    {
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

        public Func<Credentials> CredentialsProvider { get; }
        public string BaseUrl { get; }

        public Users.UserService Users { get; }
        public Accounts.AccountService Accounts { get; }
        public Categories.CategoryService Categories { get; }
        public CreditCards.CreditCardService CreditCards { get; }
        public Invoices.InvoiceService Invoices { get; }
        public Transactions.TransactionService Transactions { get; }
        public Transfers.TransferService Transfers { get; }
        public Budgets.BudgetService Budgets { get; }

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

        public NOrganizzeClient(HttpClient httpClient, string email, string apiKey, string baseUrl = OrganizzeRestV2Url)
            : this(httpClient, () => new Credentials(email, apiKey), baseUrl)
        {
        }

        public NOrganizzeClient(Func<Credentials> credentialsProvider, string baseUrl = OrganizzeRestV2Url)
            : this(new HttpClient(), credentialsProvider, baseUrl)
        {
            _disposeHttpClient = true;
        }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
