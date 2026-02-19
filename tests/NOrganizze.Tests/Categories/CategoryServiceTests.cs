using NOrganizze.Categories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NOrganizze.Tests.Categories
{
    public class CategoryServiceTests
    {
        private readonly NOrganizzeClientFixture _fixture;
        private readonly ITestContextAccessor _testContextAccessor;

        public CategoryServiceTests(NOrganizzeClientFixture fixture, ITestContextAccessor testContextAccessor)
        {
            _fixture = fixture;
            _testContextAccessor = testContextAccessor;
        }

        [Fact]
        public void Test_CategoryService_CrudOperations_Sync()
        {
            var guid = Guid.NewGuid();
            var client = _fixture.Client;

            var category = client.Categories.Create(BuildCategoryCreateOptions(guid));
            AssertCategoryProperties(category, guid);

            category = client.Categories.Get(category.Id);
            AssertCategoryProperties(category, guid);

            category = client.Categories.Update(category.Id, new CategoryUpdateOptions
            {
                Name = $"Test Category {guid}"
            });

            var categories = client.Categories.List();
            category = categories.Single(m => m.Id == category.Id);
            AssertCategoryProperties(category, guid);

            category = client.Categories.Delete(category.Id);
            AssertCategoryProperties(category, guid);

            categories = client.Categories.List();
            Assert.DoesNotContain(categories, m => m.Id == category.Id);
        }

        [Fact]
        public async Task Test_CategoryService_CrudOperations_Async()
        {
            var guid = Guid.NewGuid();
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;

            var category = await client.Categories.CreateAsync(BuildCategoryCreateOptions(guid), requestOptions, cancellationToken);
            AssertCategoryProperties(category, guid);

            category = await client.Categories.GetAsync(category.Id, requestOptions, cancellationToken);
            AssertCategoryProperties(category, guid);

            category = await client.Categories.UpdateAsync(category.Id, new CategoryUpdateOptions
            {
                Name = $"Test Category {guid}"
            }, requestOptions, cancellationToken);

            var categories = await client.Categories.ListAsync(requestOptions, cancellationToken);
            category = categories.Single(m => m.Id == category.Id);
            AssertCategoryProperties(category, guid);

            category = await client.Categories.DeleteAsync(category.Id, null, requestOptions, cancellationToken);
            AssertCategoryProperties(category, guid);

            categories = await client.Categories.ListAsync(requestOptions, cancellationToken);
            Assert.DoesNotContain(categories, m => m.Id == category.Id);
        }

        [Fact]
        public void Test_CategoryService_DeleteWithReplacement_Sync()
        {
            // Arrange
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var client = _fixture.Client;
            var category1 = client.Categories.Create(BuildCategoryCreateOptions(guid1));
            var category2 = client.Categories.Create(BuildCategoryCreateOptions(guid2));

            // Act
            var deletedCategory = client.Categories.Delete(category1.Id, new CategoryDeleteOptions
            {
                ReplacementId = category2.Id
            });

            // Assert
            Assert.Equal(category1.Id, deletedCategory.Id);

            // Clean up
            client.Categories.Delete(category2.Id);
        }

        [Fact]
        public async Task Test_CategoryService_DeleteWithReplacement_Async()
        {
            // Arrange
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;
            var category1 = await client.Categories.CreateAsync(BuildCategoryCreateOptions(guid1), requestOptions, cancellationToken);
            var category2 = await client.Categories.CreateAsync(BuildCategoryCreateOptions(guid2), requestOptions, cancellationToken);

            // Act
            var deletedCategory = await client.Categories.DeleteAsync(category1.Id, new CategoryDeleteOptions
            {
                ReplacementId = category2.Id
            }, requestOptions, cancellationToken);

            // Assert
            Assert.Equal(category1.Id, deletedCategory.Id);

            // Clean up
            await client.Categories.DeleteAsync(category2.Id, null, requestOptions, cancellationToken);
        }

        private static void AssertCategoryProperties(Category category, Guid guid)
        {
            Assert.NotNull(category);
            Assert.Equal($"Test Category {guid}", category.Name);
            Assert.True(category.Id > 0);
        }

        private static CategoryCreateOptions BuildCategoryCreateOptions(Guid guid) => new CategoryCreateOptions
        {
            Name = $"Test Category {guid}"
        };
    }
}
