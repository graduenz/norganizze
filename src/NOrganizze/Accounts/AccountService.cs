using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Accounts
{
    /// <summary>Service for listing, creating, updating, and deleting bank accounts.</summary>
    public class AccountService : Service
    {
        private const string Accounts = "accounts";

        /// <summary>Initializes a new instance of the <see cref="AccountService"/> class.</summary>
        public AccountService(NOrganizzeClient client) : base(client)
        {
        }

        /// <summary>Lists all accounts.</summary>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>List of accounts.</returns>
        public List<Account> List(RequestOptions requestOptions = null)
        {
            return Get<List<Account>>(Accounts, requestOptions);
        }

        /// <summary>Lists all accounts asynchronously.</summary>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of accounts.</returns>
        public Task<List<Account>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Account>>(Accounts, requestOptions, cancellationToken);
        }

        /// <summary>Gets an account by id.</summary>
        /// <param name="id">Account id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The account.</returns>
        public Account Get(long id, RequestOptions requestOptions = null)
        {
            return Get<Account>($"{Accounts}/{id}", requestOptions);
        }

        /// <summary>Gets an account by id asynchronously.</summary>
        /// <param name="id">Account id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The account.</returns>
        public Task<Account> GetAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Account>($"{Accounts}/{id}", requestOptions, cancellationToken);
        }

        /// <summary>Creates a new account. Use <see cref="AccountCreateOptions"/> with name, type, and optional description and default flag.</summary>
        /// <param name="options">Required. Use <see cref="AccountCreateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The created account.</returns>
        public Account Create(AccountCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<Account>(Accounts, options, requestOptions);
        }

        /// <summary>Creates a new account asynchronously.</summary>
        /// <param name="options">Required. Use <see cref="AccountCreateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created account.</returns>
        public Task<Account> CreateAsync(AccountCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<Account>(Accounts, options, requestOptions, cancellationToken);
        }

        /// <summary>Updates an account. Use <see cref="AccountUpdateOptions"/> with the fields to update.</summary>
        /// <param name="id">Account id to update.</param>
        /// <param name="options">Required. Use <see cref="AccountUpdateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The updated account.</returns>
        public Account Update(long id, AccountUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<Account>($"{Accounts}/{id}", options, requestOptions);
        }

        /// <summary>Updates an account asynchronously.</summary>
        /// <param name="id">Account id to update.</param>
        /// <param name="options">Required. Use <see cref="AccountUpdateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated account.</returns>
        public Task<Account> UpdateAsync(long id, AccountUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<Account>($"{Accounts}/{id}", options, requestOptions, cancellationToken);
        }

        /// <summary>Deletes an account by id.</summary>
        /// <param name="id">Account id to delete.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The deleted account response.</returns>
        public Account Delete(long id, RequestOptions requestOptions = null)
        {
            return Delete<Account>($"{Accounts}/{id}", requestOptions);
        }

        /// <summary>Deletes an account by id asynchronously.</summary>
        /// <param name="id">Account id to delete.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The deleted account response.</returns>
        public Task<Account> DeleteAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return DeleteAsync<Account>($"{Accounts}/{id}", requestOptions, cancellationToken);
        }
    }
}
