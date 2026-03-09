using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.CreditCards
{
    /// <summary>Service for listing, creating, updating, and deleting credit cards.</summary>
    public class CreditCardService : Service
    {
        private const string CreditCards = "credit_cards";

        /// <summary>Initializes a new instance of the <see cref="CreditCardService"/> class.</summary>
        public CreditCardService(NOrganizzeClient client) : base(client)
        {
        }

        /// <summary>Lists all credit cards.</summary>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>List of credit cards.</returns>
        public List<CreditCard> List(RequestOptions requestOptions = null)
        {
            return Get<List<CreditCard>>(CreditCards, requestOptions);
        }

        /// <summary>Lists all credit cards asynchronously.</summary>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of credit cards.</returns>
        public Task<List<CreditCard>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<CreditCard>>(CreditCards, requestOptions, cancellationToken);
        }

        /// <summary>Gets a credit card by id.</summary>
        /// <param name="id">Credit card id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The credit card.</returns>
        public CreditCard Get(long id, RequestOptions requestOptions = null)
        {
            return Get<CreditCard>($"{CreditCards}/{id}", requestOptions);
        }

        /// <summary>Gets a credit card by id asynchronously.</summary>
        /// <param name="id">Credit card id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The credit card.</returns>
        public Task<CreditCard> GetAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<CreditCard>($"{CreditCards}/{id}", requestOptions, cancellationToken);
        }

        /// <summary>Creates a new credit card. Use <see cref="CreditCardCreateOptions"/> with name, card network, due day, and closing day.</summary>
        /// <param name="options">Required. Use <see cref="CreditCardCreateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The created credit card.</returns>
        public CreditCard Create(CreditCardCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<CreditCard>(CreditCards, options, requestOptions);
        }

        /// <summary>Creates a new credit card asynchronously.</summary>
        /// <param name="options">Required. Use <see cref="CreditCardCreateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created credit card.</returns>
        public Task<CreditCard> CreateAsync(CreditCardCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<CreditCard>(CreditCards, options, requestOptions, cancellationToken);
        }

        /// <summary>Updates a credit card. Use <see cref="CreditCardUpdateOptions"/> with the fields to update.</summary>
        /// <param name="id">Credit card id to update.</param>
        /// <param name="options">Required. Use <see cref="CreditCardUpdateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The updated credit card.</returns>
        public CreditCard Update(long id, CreditCardUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<CreditCard>($"{CreditCards}/{id}", options, requestOptions);
        }

        /// <summary>Updates a credit card asynchronously.</summary>
        /// <param name="id">Credit card id to update.</param>
        /// <param name="options">Required. Use <see cref="CreditCardUpdateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated credit card.</returns>
        public Task<CreditCard> UpdateAsync(long id, CreditCardUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<CreditCard>($"{CreditCards}/{id}", options, requestOptions, cancellationToken);
        }

        /// <summary>Deletes a credit card by id.</summary>
        /// <param name="id">Credit card id to delete.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The deleted credit card response.</returns>
        public CreditCard Delete(long id, RequestOptions requestOptions = null)
        {
            return Delete<CreditCard>($"{CreditCards}/{id}", requestOptions);
        }

        /// <summary>Deletes a credit card by id asynchronously.</summary>
        /// <param name="id">Credit card id to delete.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The deleted credit card response.</returns>
        public Task<CreditCard> DeleteAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return DeleteAsync<CreditCard>($"{CreditCards}/{id}", requestOptions, cancellationToken);
        }
    }
}
