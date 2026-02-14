using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Accounts
{
    public class AccountService : Service
    {
        private const string Accounts = "accounts";

        public AccountService(NOrganizzeClient client) : base(client)
        {
        }

        public List<Account> List(RequestOptions requestOptions = null)
        {
            return Get<List<Account>>(Accounts, requestOptions);
        }

        public Task<List<Account>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Account>>(Accounts, requestOptions, cancellationToken);
        }

        public Account Get(int id, RequestOptions requestOptions = null)
        {
            return Get<Account>($"{Accounts}/{id}", requestOptions);
        }

        public Task<Account> GetAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Account>($"{Accounts}/{id}", requestOptions, cancellationToken);
        }

        public Account Create(AccountCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<Account>(Accounts, options, requestOptions);
        }

        public Task<Account> CreateAsync(AccountCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<Account>(Accounts, options, requestOptions, cancellationToken);
        }

        public Account Update(int id, AccountUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<Account>($"{Accounts}/{id}", options, requestOptions);
        }

        public Task<Account> UpdateAsync(int id, AccountUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<Account>($"{Accounts}/{id}", options, requestOptions, cancellationToken);
        }

        public Account Delete(int id, RequestOptions requestOptions = null)
        {
            return Delete<Account>($"{Accounts}/{id}", requestOptions);
        }

        public Task<Account> DeleteAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return DeleteAsync<Account>($"{Accounts}/{id}", requestOptions, cancellationToken);
        }
    }
}
