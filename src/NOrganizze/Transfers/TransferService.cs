using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Transfers
{
    /// <summary>Service for listing, creating, updating, and deleting transfers between accounts.</summary>
    public class TransferService : Service
    {
        private const string Transfers = "transfers";

        /// <summary>Initializes a new instance of the <see cref="TransferService"/> class.</summary>
        public TransferService(NOrganizzeClient client) : base(client)
        {
        }

        /// <summary>Lists all transfers.</summary>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>List of transfers.</returns>
        public List<Transfer> List(RequestOptions requestOptions = null)
        {
            return Get<List<Transfer>>(Transfers, requestOptions);
        }

        /// <summary>Lists all transfers asynchronously.</summary>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of transfers.</returns>
        public Task<List<Transfer>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Transfer>>(Transfers, requestOptions, cancellationToken);
        }

        /// <summary>Gets a transfer by id.</summary>
        /// <param name="id">Transfer id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The transfer.</returns>
        public Transfer Get(long id, RequestOptions requestOptions = null)
        {
            return Get<Transfer>($"{Transfers}/{id}", requestOptions);
        }

        /// <summary>Gets a transfer by id asynchronously.</summary>
        /// <param name="id">Transfer id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The transfer.</returns>
        public Task<Transfer> GetAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Transfer>($"{Transfers}/{id}", requestOptions, cancellationToken);
        }

        /// <summary>Creates a new transfer. Use <see cref="TransferCreateOptions"/> with credit account, debit account, amount, and date.</summary>
        /// <param name="options">Required. Use <see cref="TransferCreateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The created transfer.</returns>
        public Transfer Create(TransferCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<Transfer>(Transfers, options, requestOptions);
        }

        /// <summary>Creates a new transfer asynchronously.</summary>
        /// <param name="options">Required. Use <see cref="TransferCreateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created transfer.</returns>
        public Task<Transfer> CreateAsync(TransferCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<Transfer>(Transfers, options, requestOptions, cancellationToken);
        }

        /// <summary>Updates a transfer. Use <see cref="TransferUpdateOptions"/> with the fields to update.</summary>
        /// <param name="id">Transfer id to update.</param>
        /// <param name="options">Required. Use <see cref="TransferUpdateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The updated transfer.</returns>
        public Transfer Update(long id, TransferUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<Transfer>($"{Transfers}/{id}", options, requestOptions);
        }

        /// <summary>Updates a transfer asynchronously.</summary>
        /// <param name="id">Transfer id to update.</param>
        /// <param name="options">Required. Use <see cref="TransferUpdateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated transfer.</returns>
        public Task<Transfer> UpdateAsync(long id, TransferUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<Transfer>($"{Transfers}/{id}", options, requestOptions, cancellationToken);
        }

        /// <summary>Deletes a transfer by id.</summary>
        /// <param name="id">Transfer id to delete.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The deleted transfer response.</returns>
        public Transfer Delete(long id, RequestOptions requestOptions = null)
        {
            return Delete<Transfer>($"{Transfers}/{id}", requestOptions);
        }

        /// <summary>Deletes a transfer by id asynchronously.</summary>
        /// <param name="id">Transfer id to delete.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The deleted transfer response.</returns>
        public Task<Transfer> DeleteAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return DeleteAsync<Transfer>($"{Transfers}/{id}", requestOptions, cancellationToken);
        }
    }
}
