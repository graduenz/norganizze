using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze
{
    public abstract class Service
    {
        protected NOrganizzeClient Client { get; }

        protected Service(NOrganizzeClient client)
        {
            Client = client;
        }

        protected T Get<T>(
            string path,
            RequestOptions requestOptions = null)
        {
            return Client.Request<T>(HttpMethod.Get, path, null, requestOptions);
        }

        protected Task<T> GetAsync<T>(
            string path,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<T>(HttpMethod.Get, path, null, requestOptions, cancellationToken);
        }

        protected T Post<T>(
            string path,
            object content = null,
            RequestOptions requestOptions = null)
        {
            return Client.Request<T>(HttpMethod.Post, path, content, requestOptions);
        }

        protected Task<T> PostAsync<T>(
            string path,
            object content = null,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<T>(HttpMethod.Post, path, content, requestOptions, cancellationToken);
        }

        protected T Put<T>(
            string path,
            object content = null,
            RequestOptions requestOptions = null)
        {
            return Client.Request<T>(HttpMethod.Put, path, content, requestOptions);
        }

        protected Task<T> PutAsync<T>(
            string path,
            object content = null,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<T>(HttpMethod.Put, path, content, requestOptions, cancellationToken);
        }

        protected T Delete<T>(
            string path,
            RequestOptions requestOptions = null)
        {
            return Client.Request<T>(HttpMethod.Delete, path, null, requestOptions);
        }

        protected Task<T> DeleteAsync<T>(
            string path,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<T>(HttpMethod.Delete, path, null, requestOptions, cancellationToken);
        }
    }
}
