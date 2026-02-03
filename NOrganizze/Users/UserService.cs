using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Users
{
    public class UserService : Service
    {
        public UserService(NOrganizzeClient client) : base(client)
        {
        }

        public User Get(int id, RequestOptions requestOptions = null)
        {
            return Get<User>($"users/{id}", requestOptions);
        }

        public async Task<User> GetAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<User>($"users/{id}", requestOptions, cancellationToken);
        }
    }
}
