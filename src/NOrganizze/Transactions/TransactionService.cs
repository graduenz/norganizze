using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Transactions
{
    /// <summary>Service for listing, creating, updating, and deleting transactions. Use <see cref="TransactionListOptions"/> with <see cref="List"/> or <see cref="ListAsync"/> to filter by date range and account.</summary>
    public class TransactionService : Service
    {
        /// <summary>Maximum number of transactions the Organizze API returns per request (server-imposed limit).</summary>
        public const int MaxTransactionsPerRequest = 500;

        private const string Transactions = "transactions";

        /// <summary>Initializes a new instance of the <see cref="TransactionService"/> class.</summary>
        public TransactionService(NOrganizzeClient client) : base(client)
        {
        }

        /// <summary>Lists transactions, optionally filtered by date range and account. Pass <paramref name="options"/> with <see cref="TransactionListOptions.StartDate"/>, <see cref="TransactionListOptions.EndDate"/>, and/or <see cref="TransactionListOptions.AccountId"/>. Unless <see cref="TransactionListOptions.AutoPaginate"/> is explicitly false, the library automatically fetches beyond the <see cref="MaxTransactionsPerRequest"/> limit by advancing the start date and merging results. Missing dates default to the current month.</summary>
        /// <param name="options">Optional. Use <see cref="TransactionListOptions"/> to filter by start date, end date, and account id. Dates are sent as yyyy-MM-dd.</param>
        /// <param name="requestOptions">Optional per-request overrides (base URL, credentials, user agent).</param>
        /// <returns>List of transactions.</returns>
        public List<Transaction> List(TransactionListOptions options = null, RequestOptions requestOptions = null)
        {
            if (ShouldAutoPaginate(options))
                return ListWithAutoPagination(options, requestOptions);

            var path = BuildListPath(options);
            return Get<List<Transaction>>(path, requestOptions);
        }

        /// <summary>Lists transactions asynchronously, optionally filtered by date range and account. Pass <paramref name="options"/> with <see cref="TransactionListOptions.StartDate"/>, <see cref="TransactionListOptions.EndDate"/>, and/or <see cref="TransactionListOptions.AccountId"/>. Unless <see cref="TransactionListOptions.AutoPaginate"/> is explicitly false, the library automatically fetches beyond the <see cref="MaxTransactionsPerRequest"/> limit by advancing the start date and merging results. Missing dates default to the current month.</summary>
        /// <param name="options">Optional. Use <see cref="TransactionListOptions"/> to filter by start date, end date, and account id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of transactions.</returns>
        public Task<List<Transaction>> ListAsync(TransactionListOptions options = null, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            if (ShouldAutoPaginate(options))
                return ListWithAutoPaginationAsync(options, requestOptions, cancellationToken);

            var path = BuildListPath(options);
            return GetAsync<List<Transaction>>(path, requestOptions, cancellationToken);
        }

        private static bool ShouldAutoPaginate(TransactionListOptions options)
        {
            if (options != null && options.AutoPaginate == false)
                return false;
            return true;
        }

        private static void GetPaginationRange(TransactionListOptions options, out DateTime cursor, out DateTime endDate)
        {
            var now = DateTime.UtcNow;
            cursor = options?.StartDate ?? new DateTime(now.Year, now.Month, 1);
            endDate = options?.EndDate ?? new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        }

        private static bool ProcessBatch(
            List<Transaction> batch,
            HashSet<long> seen,
            List<Transaction> result,
            ref DateTime cursor)
        {
            if (batch == null || batch.Count == 0)
                return false;

            int newCount = 0;
            var maxDate = DateTime.MinValue;
            foreach (var tx in batch)
            {
                if (seen.Add(tx.Id))
                {
                    result.Add(tx);
                    newCount++;
                }
                if (tx.Date > maxDate)
                    maxDate = tx.Date;
            }

            if (batch.Count < MaxTransactionsPerRequest)
                return false;
            if (maxDate <= cursor)
                return false;
            if (newCount == 0)
                return false;

            cursor = maxDate;
            return true;
        }

        private List<Transaction> ListWithAutoPagination(TransactionListOptions options, RequestOptions requestOptions)
        {
            GetPaginationRange(options, out var cursor, out var endDate);
            var seen = new HashSet<long>();
            var result = new List<Transaction>();

            while (true)
            {
                var pageOptions = new TransactionListOptions { StartDate = cursor, EndDate = endDate, AccountId = options?.AccountId, AutoPaginate = false };
                var batch = Get<List<Transaction>>(BuildListPath(pageOptions), requestOptions);
                if (!ProcessBatch(batch, seen, result, ref cursor))
                    break;
            }

            return result;
        }

        private async Task<List<Transaction>> ListWithAutoPaginationAsync(
            TransactionListOptions options,
            RequestOptions requestOptions,
            CancellationToken cancellationToken)
        {
            GetPaginationRange(options, out var cursor, out var endDate);
            var seen = new HashSet<long>();
            var result = new List<Transaction>();

            while (true)
            {
                var pageOptions = new TransactionListOptions { StartDate = cursor, EndDate = endDate, AccountId = options?.AccountId, AutoPaginate = false };
                var batch = await GetAsync<List<Transaction>>(BuildListPath(pageOptions), requestOptions, cancellationToken).ConfigureAwait(false);
                if (!ProcessBatch(batch, seen, result, ref cursor))
                    break;
            }

            return result;
        }

        private static string BuildListPath(TransactionListOptions options)
        {
            var path = Transactions;
            if (options != null)
            {
                var queryParams = new List<string>();
                if (options.StartDate.HasValue)
                    queryParams.Add($"start_date={UrlEncode(options.StartDate.Value.ToString("yyyy-MM-dd"))}");
                if (options.EndDate.HasValue)
                    queryParams.Add($"end_date={UrlEncode(options.EndDate.Value.ToString("yyyy-MM-dd"))}");
                if (options.AccountId.HasValue)
                    queryParams.Add($"account_id={options.AccountId.Value}");

                if (queryParams.Count > 0)
                    path += "?" + string.Join("&", queryParams);
            }

            return path;
        }

        /// <summary>Gets a single transaction by id.</summary>
        /// <param name="id">Transaction id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The transaction, or null if not found.</returns>
        public Transaction Get(long id, RequestOptions requestOptions = null)
        {
            return Get<Transaction>($"{Transactions}/{id}", requestOptions);
        }

        /// <summary>Gets a single transaction by id asynchronously.</summary>
        /// <param name="id">Transaction id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The transaction, or null if not found.</returns>
        public Task<Transaction> GetAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Transaction>($"{Transactions}/{id}", requestOptions, cancellationToken);
        }

        /// <summary>Creates a new transaction. Pass required and optional fields via <see cref="TransactionCreateOptions"/>.</summary>
        /// <param name="options">Required. Use <see cref="TransactionCreateOptions"/> with at least description, date, and amount/account/category as needed.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The created transaction.</returns>
        public Transaction Create(TransactionCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<Transaction>(Transactions, options, requestOptions);
        }

        /// <summary>Creates a new transaction asynchronously.</summary>
        /// <param name="options">Required. Use <see cref="TransactionCreateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created transaction.</returns>
        public Task<Transaction> CreateAsync(TransactionCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<Transaction>(Transactions, options, requestOptions, cancellationToken);
        }

        /// <summary>Updates an existing transaction. Pass only the fields to update via <see cref="TransactionUpdateOptions"/>.</summary>
        /// <param name="id">Transaction id to update.</param>
        /// <param name="options">Required. Use <see cref="TransactionUpdateOptions"/> with the fields to update.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The updated transaction.</returns>
        public Transaction Update(long id, TransactionUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<Transaction>($"{Transactions}/{id}", options, requestOptions);
        }

        /// <summary>Updates an existing transaction asynchronously.</summary>
        /// <param name="id">Transaction id to update.</param>
        /// <param name="options">Required. Use <see cref="TransactionUpdateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated transaction.</returns>
        public Task<Transaction> UpdateAsync(long id, TransactionUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<Transaction>($"{Transactions}/{id}", options, requestOptions, cancellationToken);
        }

        /// <summary>Deletes a transaction. Pass <paramref name="options"/> for recurring transactions when the API supports update_future/update_all.</summary>
        /// <param name="id">Transaction id to delete.</param>
        /// <param name="options">Optional. Use <see cref="TransactionDeleteOptions"/> for recurring transactions.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The deleted transaction response.</returns>
        public Transaction Delete(long id, TransactionDeleteOptions options = null, RequestOptions requestOptions = null)
        {
            return Client.Request<Transaction>(HttpMethod.Delete, $"{Transactions}/{id}", options, requestOptions);
        }

        /// <summary>Deletes a transaction asynchronously.</summary>
        /// <param name="id">Transaction id to delete.</param>
        /// <param name="options">Optional. Use <see cref="TransactionDeleteOptions"/> for recurring transactions.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The deleted transaction response.</returns>
        public Task<Transaction> DeleteAsync(long id, TransactionDeleteOptions options = null, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<Transaction>(HttpMethod.Delete, $"{Transactions}/{id}", options, requestOptions, cancellationToken);
        }
    }
}
