using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#if NETSTANDARD2_0 || NET472
using System.Net;
#endif

namespace NOrganizze
{
    /// <summary>Base class for Organizze API service facades (e.g. <see cref="Transactions.TransactionService"/>, <see cref="Accounts.AccountService"/>). Used internally; consumers use the service properties on <see cref="NOrganizzeClient"/>.</summary>
    public abstract class Service
    {
        /// <summary>The client used to perform API requests.</summary>
        protected NOrganizzeClient Client { get; }

        /// <summary>Initializes the service with the given client.</summary>
        protected Service(NOrganizzeClient client)
        {
            Client = client;
        }

        /// <summary>Performs a GET request and deserializes the response.</summary>
        protected T Get<T>(
            string path,
            RequestOptions requestOptions = null)
        {
            return Client.Request<T>(HttpMethod.Get, path, null, requestOptions);
        }

        /// <summary>Performs a GET request asynchronously and deserializes the response.</summary>
        protected Task<T> GetAsync<T>(
            string path,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<T>(HttpMethod.Get, path, null, requestOptions, cancellationToken);
        }

        /// <summary>Performs a POST request and deserializes the response.</summary>
        protected T Post<T>(
            string path,
            object content = null,
            RequestOptions requestOptions = null)
        {
            return Client.Request<T>(HttpMethod.Post, path, content, requestOptions);
        }

        /// <summary>Performs a POST request asynchronously and deserializes the response.</summary>
        protected Task<T> PostAsync<T>(
            string path,
            object content = null,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<T>(HttpMethod.Post, path, content, requestOptions, cancellationToken);
        }

        /// <summary>Performs a PUT request and deserializes the response.</summary>
        protected T Put<T>(
            string path,
            object content = null,
            RequestOptions requestOptions = null)
        {
            return Client.Request<T>(HttpMethod.Put, path, content, requestOptions);
        }

        /// <summary>Performs a PUT request asynchronously and deserializes the response.</summary>
        protected Task<T> PutAsync<T>(
            string path,
            object content = null,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<T>(HttpMethod.Put, path, content, requestOptions, cancellationToken);
        }

        /// <summary>Performs a DELETE request and deserializes the response.</summary>
        protected T Delete<T>(
            string path,
            RequestOptions requestOptions = null)
        {
            return Client.Request<T>(HttpMethod.Delete, path, null, requestOptions);
        }

        /// <summary>Performs a DELETE request with no response body.</summary>
        protected void Delete(
            string path,
            RequestOptions requestOptions = null)
        {
            Client.Request(HttpMethod.Delete, path, null, requestOptions);
        }

        /// <summary>Performs a DELETE request asynchronously and deserializes the response.</summary>
        protected Task<T> DeleteAsync<T>(
            string path,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<T>(HttpMethod.Delete, path, null, requestOptions, cancellationToken);
        }

        /// <summary>Performs a DELETE request asynchronously with no response body.</summary>
        protected Task DeleteAsync(
            string path,
            RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync(HttpMethod.Delete, path, null, requestOptions, cancellationToken);
        }

        /// <summary>URL-encodes a string for use in query parameters.</summary>
        protected static string UrlEncode(string value)
        {
#if NETSTANDARD2_0 || NET472
            return WebUtility.UrlEncode(value);
#else
            return Uri.EscapeDataString(value);
#endif
        }
    }
}
