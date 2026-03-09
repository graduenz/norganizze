using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Categories
{
    /// <summary>Service for listing, creating, updating, and deleting categories. Use <see cref="CategoryDeleteOptions"/> with <see cref="Delete"/> or <see cref="DeleteAsync"/> when the API requires a replacement category.</summary>
    public class CategoryService : Service
    {
        private const string Categories = "categories";

        /// <summary>Initializes a new instance of the <see cref="CategoryService"/> class.</summary>
        public CategoryService(NOrganizzeClient client) : base(client)
        {
        }

        /// <summary>Lists all categories.</summary>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>List of categories.</returns>
        public List<Category> List(RequestOptions requestOptions = null)
        {
            return Get<List<Category>>(Categories, requestOptions);
        }

        /// <summary>Lists all categories asynchronously.</summary>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of categories.</returns>
        public Task<List<Category>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Category>>(Categories, requestOptions, cancellationToken);
        }

        /// <summary>Gets a category by id.</summary>
        /// <param name="id">Category id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The category.</returns>
        public Category Get(long id, RequestOptions requestOptions = null)
        {
            return Get<Category>($"{Categories}/{id}", requestOptions);
        }

        /// <summary>Gets a category by id asynchronously.</summary>
        /// <param name="id">Category id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The category.</returns>
        public Task<Category> GetAsync(long id, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Category>($"{Categories}/{id}", requestOptions, cancellationToken);
        }

        /// <summary>Creates a new category. Use <see cref="CategoryCreateOptions"/> with name.</summary>
        /// <param name="options">Required. Use <see cref="CategoryCreateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The created category.</returns>
        public Category Create(CategoryCreateOptions options, RequestOptions requestOptions = null)
        {
            return Post<Category>(Categories, options, requestOptions);
        }

        /// <summary>Creates a new category asynchronously.</summary>
        /// <param name="options">Required. Use <see cref="CategoryCreateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created category.</returns>
        public Task<Category> CreateAsync(CategoryCreateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PostAsync<Category>(Categories, options, requestOptions, cancellationToken);
        }

        /// <summary>Updates a category. Use <see cref="CategoryUpdateOptions"/> with the fields to update.</summary>
        /// <param name="id">Category id to update.</param>
        /// <param name="options">Required. Use <see cref="CategoryUpdateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The updated category.</returns>
        public Category Update(long id, CategoryUpdateOptions options, RequestOptions requestOptions = null)
        {
            return Put<Category>($"{Categories}/{id}", options, requestOptions);
        }

        /// <summary>Updates a category asynchronously.</summary>
        /// <param name="id">Category id to update.</param>
        /// <param name="options">Required. Use <see cref="CategoryUpdateOptions"/>.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated category.</returns>
        public Task<Category> UpdateAsync(long id, CategoryUpdateOptions options, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return PutAsync<Category>($"{Categories}/{id}", options, requestOptions, cancellationToken);
        }

        /// <summary>Deletes a category. Pass <paramref name="options"/> with <see cref="CategoryDeleteOptions.ReplacementId"/> when the API requires a replacement category for existing transactions.</summary>
        /// <param name="id">Category id to delete.</param>
        /// <param name="options">Optional. Use <see cref="CategoryDeleteOptions"/> when a replacement category is required.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The deleted category response.</returns>
        public Category Delete(long id, CategoryDeleteOptions options = null, RequestOptions requestOptions = null)
        {
            return Client.Request<Category>(HttpMethod.Delete, $"{Categories}/{id}", options, requestOptions);
        }

        /// <summary>Deletes a category asynchronously.</summary>
        /// <param name="id">Category id to delete.</param>
        /// <param name="options">Optional. Use <see cref="CategoryDeleteOptions"/> when a replacement category is required.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The deleted category response.</returns>
        public Task<Category> DeleteAsync(long id, CategoryDeleteOptions options = null, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return Client.RequestAsync<Category>(HttpMethod.Delete, $"{Categories}/{id}", options, requestOptions, cancellationToken);
        }
    }
}

