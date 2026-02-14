using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Users
{
    public class UserService : Service
    {
        private const string Users = "users";

        public UserService(NOrganizzeClient client) : base(client)
        {
        }

        public User Get(int id, RequestOptions requestOptions = null)
        {
            return Get<User>($"{Users}/{id}", requestOptions);
        }

        public async Task<User> GetAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<User>($"{Users}/{id}", requestOptions, cancellationToken);
        }
    }
}
