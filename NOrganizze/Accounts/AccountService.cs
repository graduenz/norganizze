using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Accounts
{
    public class AccountService : Service
    {
        public AccountService(NOrganizzeClient client) : base(client)
        {
        }

        public List<Account> List(RequestOptions requestOptions = null)
        {
            return Get<List<Account>>("accounts", requestOptions);
        }

        public Task<List<Account>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Account>>("accounts", requestOptions, cancellationToken);
        }

        public Account Get(int id, RequestOptions requestOptions = null)
        {
            return Get<Account>($"accounts/{id}", requestOptions);
        }

        public Task<Account> GetAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Account>($"accounts/{id}", requestOptions, cancellationToken);
        }

        public Account Create(AccountCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<Account>("accounts", options, requestOptions);
        }

        public Task<Account> CreateAsync(AccountCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<Account>("accounts", options, requestOptions, cancellationToken);
        }

        public Account Update(int id, AccountUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<Account>($"accounts/{id}", options, requestOptions);
        }

        public Task<Account> UpdateAsync(int id, AccountUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<Account>($"accounts/{id}", options, requestOptions, cancellationToken);
        }

        public Account Delete(int id, RequestOptions requestOptions = null)
        {
            return Delete<Account>($"accounts/{id}", requestOptions);
        }

        public Task<Account> DeleteAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return DeleteAsync<Account>($"accounts/{id}", requestOptions, cancellationToken);
        }
    }
}
