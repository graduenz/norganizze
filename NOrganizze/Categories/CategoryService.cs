using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Categories
{
    public class CategoryService : Service
    {
        private const string Categories = "categories";

        public CategoryService(NOrganizzeClient client) : base(client)
        {
        }

        public List<Category> List(RequestOptions requestOptions = null)
        {
            return Get<List<Category>>(Categories, requestOptions);
        }

        public Task<List<Category>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Category>>(Categories, requestOptions, cancellationToken);
        }

        public Category Get(long id, RequestOptions requestOptions = null)
        {
            return Get<Category>($"{Categories}/{id}", requestOptions);
        }

        public Task<Category> GetAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Category>($"{Categories}/{id}", requestOptions, cancellationToken);
        }

        public Category Create(CategoryCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<Category>(Categories, options, requestOptions);
        }

        public Task<Category> CreateAsync(CategoryCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<Category>(Categories, options, requestOptions, cancellationToken);
        }

        public Category Update(long id, CategoryUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<Category>($"{Categories}/{id}", options, requestOptions);
        }

        public Task<Category> UpdateAsync(long id, CategoryUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<Category>($"{Categories}/{id}", options, requestOptions, cancellationToken);
        }

        public Category Delete(long id, CategoryDeleteOptions options = null, RequestOptions requestOptions = null)
        {
            return Client.Request<Category>(HttpMethod.Delete, $"{Categories}/{id}", options, requestOptions);
        }

        public Task<Category> DeleteAsync(long id, CategoryDeleteOptions options = null, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<Category>(HttpMethod.Delete, $"{Categories}/{id}", options, requestOptions, cancellationToken);
        }
    }
}

