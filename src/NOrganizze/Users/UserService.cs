using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Users
{
    /// <summary>Service for accessing user information.</summary>
    public class UserService : Service
    {
        private const string Users = "users";

        /// <summary>Initializes a new instance of the <see cref="UserService"/> class.</summary>
        public UserService(NOrganizzeClient client) : base(client)
        {
        }

        /// <summary>Gets a user by id.</summary>
        /// <param name="id">User id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The user.</returns>
        public User Get(long id, RequestOptions requestOptions = null)
        {
            return Get<User>($"{Users}/{id}", requestOptions);
        }

        /// <summary>Gets a user by id asynchronously.</summary>
        /// <param name="id">User id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The user.</returns>
        public async Task<User> GetAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<User>($"{Users}/{id}", requestOptions, cancellationToken);
        }
    }
}
