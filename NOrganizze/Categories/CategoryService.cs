using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Categories
{
    public class CategoryService : Service
    {
        public CategoryService(NOrganizzeClient client) : base(client)
        {
        }

        public List<Category> List(RequestOptions requestOptions = null)
        {
            return Get<List<Category>>("categories", requestOptions);
        }

        public Task<List<Category>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Category>>("categories", requestOptions, cancellationToken);
        }

        public Category Get(int id, RequestOptions requestOptions = null)
        {
            return Get<Category>($"categories/{id}", requestOptions);
        }

        public Task<Category> GetAsync(int id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Category>($"categories/{id}", requestOptions, cancellationToken);
        }

        public Category Create(CategoryCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<Category>("categories", options, requestOptions);
        }

        public Task<Category> CreateAsync(CategoryCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<Category>("categories", options, requestOptions, cancellationToken);
        }

        public Category Update(int id, CategoryUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<Category>($"categories/{id}", options, requestOptions);
        }

        public Task<Category> UpdateAsync(int id, CategoryUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<Category>($"categories/{id}", options, requestOptions, cancellationToken);
        }

        public Category Delete(int id, CategoryDeleteOptions options = null, RequestOptions requestOptions = null)
        {
            return Client.Request<Category>(HttpMethod.Delete, $"categories/{id}", options, requestOptions);
        }

        public Task<Category> DeleteAsync(int id, CategoryDeleteOptions options = null, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<Category>(HttpMethod.Delete, $"categories/{id}", options, requestOptions, cancellationToken);
        }
    }
}

