using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Transfers
{
    public class TransferService : Service
    {
        public TransferService(NOrganizzeClient client) : base(client)
        {
        }

        public List<Transfer> List(RequestOptions requestOptions = null)
        {
            return Get<List<Transfer>>("transfers", requestOptions);
        }

        public Task<List<Transfer>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Transfer>>("transfers", requestOptions, cancellationToken);
        }

        public Transfer Get(int id, RequestOptions requestOptions = null)
        {
            return Get<Transfer>($"transfers/{id}", requestOptions);
        }

        public Task<Transfer> GetAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Transfer>($"transfers/{id}", requestOptions, cancellationToken);
        }

        public Transfer Create(TransferCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<Transfer>("transfers", options, requestOptions);
        }

        public Task<Transfer> CreateAsync(TransferCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<Transfer>("transfers", options, requestOptions, cancellationToken);
        }

        public Transfer Update(int id, TransferUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<Transfer>($"transfers/{id}", options, requestOptions);
        }

        public Task<Transfer> UpdateAsync(int id, TransferUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<Transfer>($"transfers/{id}", options, requestOptions, cancellationToken);
        }

        public Transfer Delete(int id, RequestOptions requestOptions = null)
        {
            return Delete<Transfer>($"transfers/{id}", requestOptions);
        }

        public Task<Transfer> DeleteAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return DeleteAsync<Transfer>($"transfers/{id}", requestOptions, cancellationToken);
        }
    }
}
