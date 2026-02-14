using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Transfers
{
    public class TransferService : Service
    {
        private const string Transfers = "transfers";

        public TransferService(NOrganizzeClient client) : base(client)
        {
        }

        public List<Transfer> List(RequestOptions requestOptions = null)
        {
            return Get<List<Transfer>>(Transfers, requestOptions);
        }

        public Task<List<Transfer>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Transfer>>(Transfers, requestOptions, cancellationToken);
        }

        public Transfer Get(int id, RequestOptions requestOptions = null)
        {
            return Get<Transfer>($"{Transfers}/{id}", requestOptions);
        }

        public Task<Transfer> GetAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Transfer>($"{Transfers}/{id}", requestOptions, cancellationToken);
        }

        public Transfer Create(TransferCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<Transfer>(Transfers, options, requestOptions);
        }

        public Task<Transfer> CreateAsync(TransferCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<Transfer>(Transfers, options, requestOptions, cancellationToken);
        }

        public Transfer Update(int id, TransferUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<Transfer>($"{Transfers}/{id}", options, requestOptions);
        }

        public Task<Transfer> UpdateAsync(int id, TransferUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<Transfer>($"{Transfers}/{id}", options, requestOptions, cancellationToken);
        }

        public Transfer Delete(int id, RequestOptions requestOptions = null)
        {
            return Delete<Transfer>($"{Transfers}/{id}", requestOptions);
        }

        public Task<Transfer> DeleteAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return DeleteAsync<Transfer>($"{Transfers}/{id}", requestOptions, cancellationToken);
        }
    }
}
