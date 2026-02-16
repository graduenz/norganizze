using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Transactions
{
    public class TransactionService : Service
    {
        private const string Transactions = "transactions";

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

        public Transaction Get(long id, RequestOptions requestOptions = null)
        {
            return Get<Transaction>($"{Transactions}/{id}", requestOptions);
        }

        public Task<Transaction> GetAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Transaction>($"{Transactions}/{id}", requestOptions, cancellationToken);
        }

        public Transaction Create(TransactionCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<Transaction>(Transactions, options, requestOptions);
        }

        public Task<Transaction> CreateAsync(TransactionCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<Transaction>(Transactions, options, requestOptions, cancellationToken);
        }

        public Transaction Update(long id, TransactionUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<Transaction>($"{Transactions}/{id}", options, requestOptions);
        }

        public Task<Transaction> UpdateAsync(long id, TransactionUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<Transaction>($"{Transactions}/{id}", options, requestOptions, cancellationToken);
        }

        public Transaction Delete(long id, TransactionDeleteOptions options = null, RequestOptions requestOptions = null)
        {
            return Client.Request<Transaction>(HttpMethod.Delete, $"{Transactions}/{id}", options, requestOptions);
        }

        public Task<Transaction> DeleteAsync(long id, TransactionDeleteOptions options = null, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<Transaction>(HttpMethod.Delete, $"{Transactions}/{id}", options, requestOptions, cancellationToken);
        }
    }
}
