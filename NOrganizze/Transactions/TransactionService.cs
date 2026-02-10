using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Transactions
{
    public class TransactionService : Service
    {
        public TransactionService(NOrganizzeClient client) : base(client)
        {
        }

        public List<Transaction> List(TransactionListOptions options = null, RequestOptions requestOptions = null)
        {
            var path = BuildListPath(options);
            return Get<List<Transaction>>(path, requestOptions);
        }

        public Task<List<Transaction>> ListAsync(TransactionListOptions options = null, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            var path = BuildListPath(options);
            return GetAsync<List<Transaction>>(path, requestOptions, cancellationToken);
        }

        private static string BuildListPath(TransactionListOptions options)
        {
            var path = "transactions";
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

        public Transaction Get(int id, RequestOptions requestOptions = null)
        {
            return Get<Transaction>($"transactions/{id}", requestOptions);
        }

        public Task<Transaction> GetAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Transaction>($"transactions/{id}", requestOptions, cancellationToken);
        }

        public Transaction Create(TransactionCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<Transaction>("transactions", options, requestOptions);
        }

        public Task<Transaction> CreateAsync(TransactionCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<Transaction>("transactions", options, requestOptions, cancellationToken);
        }

        public Transaction Update(int id, TransactionUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<Transaction>($"transactions/{id}", options, requestOptions);
        }

        public Task<Transaction> UpdateAsync(int id, TransactionUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<Transaction>($"transactions/{id}", options, requestOptions, cancellationToken);
        }

        public Transaction Delete(int id, TransactionDeleteOptions options = null, RequestOptions requestOptions = null)
        {
            return Client.Request<Transaction>(HttpMethod.Delete, $"transactions/{id}", options, requestOptions);
        }

        public Task<Transaction> DeleteAsync(int id, TransactionDeleteOptions options = null, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<Transaction>(HttpMethod.Delete, $"transactions/{id}", options, requestOptions, cancellationToken);
        }
    }
}
