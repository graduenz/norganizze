using NOrganizze.Accounts;
using NOrganizze.Categories;
using NOrganizze.Transactions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NOrganizze.Tests.Transactions
{
    public class TransactionServiceTests
    {
        private readonly NOrganizzeClientFixture _fixture;
        private readonly ITestContextAccessor _testContextAccessor;

        public TransactionServiceTests(NOrganizzeClientFixture fixture, ITestContextAccessor testContextAccessor)
        {
            _fixture = fixture;
            _testContextAccessor = testContextAccessor;
        }

        [Fact]
        public void Test_TransactionService_CrudOperations_Sync()
        {
            var guid = Guid.NewGuid();
            var client = _fixture.Client;

            var account = GetOrCreateTestAccount(client, guid);
            var category = GetOrCreateTestCategory(client, guid);

            var transaction = client.Transactions.Create(BuildTransactionCreateOptions(guid, account.Id, category.Id));
            AssertTransactionProperties(transaction, guid, account.Id, category.Id);

            transaction = client.Transactions.Get(transaction.Id);
            AssertTransactionProperties(transaction, guid, account.Id, category.Id);

            transaction = client.Transactions.Update(transaction.Id, new TransactionUpdateOptions
            {
                Description = $"Test Transaction {guid}",
                AmountCents = -20000,
                Paid = true
            });

            var transactions = client.Transactions.List();
            transaction = transactions.Single(m => m.Id == transaction.Id);
            Assert.Equal($"Test Transaction {guid}", transaction.Description);
            Assert.Equal(-20000, transaction.AmountCents);
            Assert.True(transaction.Paid);

            transaction = client.Transactions.Delete(transaction.Id);
            Assert.Equal($"Test Transaction {guid}", transaction.Description);

            transactions = client.Transactions.List();
            Assert.DoesNotContain(transactions, m => m.Id == transaction.Id);

            // Clean up
            CleanupTestData(client, account.Id, category.Id);
        }

        [Fact]
        public async Task Test_TransactionService_CrudOperations_Async()
        {
            var guid = Guid.NewGuid();
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;

            var account = await GetOrCreateTestAccountAsync(client, guid, requestOptions, cancellationToken);
            var category = await GetOrCreateTestCategoryAsync(client, guid, requestOptions, cancellationToken);

            var transaction = await client.Transactions.CreateAsync(BuildTransactionCreateOptions(guid, account.Id, category.Id), requestOptions, cancellationToken);
            AssertTransactionProperties(transaction, guid, account.Id, category.Id);

            transaction = await client.Transactions.GetAsync(transaction.Id, requestOptions, cancellationToken);
            AssertTransactionProperties(transaction, guid, account.Id, category.Id);

            transaction = await client.Transactions.UpdateAsync(transaction.Id, new TransactionUpdateOptions
            {
                Description = $"Test Transaction {guid}",
                AmountCents = -20000,
                Paid = true
            }, requestOptions, cancellationToken);

            var transactions = await client.Transactions.ListAsync(null, requestOptions, cancellationToken);
            transaction = transactions.Single(m => m.Id == transaction.Id);
            Assert.Equal($"Test Transaction {guid}", transaction.Description);
            Assert.Equal(-20000, transaction.AmountCents);
            Assert.True(transaction.Paid);

            transaction = await client.Transactions.DeleteAsync(transaction.Id, null, requestOptions, cancellationToken);
            Assert.Equal($"Test Transaction {guid}", transaction.Description);

            transactions = await client.Transactions.ListAsync(null, requestOptions, cancellationToken);
            Assert.DoesNotContain(transactions, m => m.Id == transaction.Id);

            // Clean up
            await CleanupTestDataAsync(client, account.Id, category.Id, requestOptions, cancellationToken);
        }

        [Fact]
        public void Test_TransactionService_ListWithFilters_Sync()
        {
            // Arrange
            var client = _fixture.Client;
            var now = DateTime.UtcNow;
            var options = new TransactionListOptions
            {
                StartDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59, DateTimeKind.Utc)
            };

            // Act
            var transactions = client.Transactions.List(options);

            // Assert
            Assert.NotNull(transactions);
            if (transactions.Any())
            {
                foreach (var transaction in transactions)
                {
                    Assert.True(transaction.Date >= options.StartDate);
                    Assert.True(transaction.Date <= options.EndDate);
                }
            }
        }

        [Fact]
        public async Task Test_TransactionService_ListWithFilters_Async()
        {
            // Arrange
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;
            var now = DateTime.UtcNow;
            var options = new TransactionListOptions
            {
                StartDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59, DateTimeKind.Utc)
            };

            // Act
            var transactions = await client.Transactions.ListAsync(options, requestOptions, cancellationToken);

            // Assert
            Assert.NotNull(transactions);
            if (transactions.Any())
            {
                foreach (var transaction in transactions)
                {
                    Assert.True(transaction.Date >= options.StartDate);
                    Assert.True(transaction.Date <= options.EndDate);
                }
            }
        }

        [Fact]
        public void Test_TransactionService_WithTags_Sync()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var client = _fixture.Client;
            var account = GetOrCreateTestAccount(client, guid);
            var category = GetOrCreateTestCategory(client, guid);
            var options = BuildTransactionCreateOptions(guid, account.Id, category.Id);
            options.Tags = new System.Collections.Generic.List<Tag>
            {
                new Tag { Name = "test-tag" },
                new Tag { Name = "automation" }
            };

            // Act
            var transaction = client.Transactions.Create(options);

            // Assert
            Assert.NotNull(transaction.Tags);
            Assert.True(transaction.Tags.Count >= 2);

            // Clean up
            client.Transactions.Delete(transaction.Id);
            CleanupTestData(client, account.Id, category.Id);
        }

        [Fact]
        public async Task Test_TransactionService_WithTags_Async()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;
            var account = await GetOrCreateTestAccountAsync(client, guid, requestOptions, cancellationToken);
            var category = await GetOrCreateTestCategoryAsync(client, guid, requestOptions, cancellationToken);
            var options = BuildTransactionCreateOptions(guid, account.Id, category.Id);
            options.Tags = new System.Collections.Generic.List<Tag>
            {
                new Tag { Name = "test-tag" },
                new Tag { Name = "automation" }
            };

            // Act
            var transaction = await client.Transactions.CreateAsync(options, requestOptions, cancellationToken);

            // Assert
            Assert.NotNull(transaction.Tags);
            Assert.True(transaction.Tags.Count >= 2);

            // Clean up
            await client.Transactions.DeleteAsync(transaction.Id, null, requestOptions, cancellationToken);
            await CleanupTestDataAsync(client, account.Id, category.Id, requestOptions, cancellationToken);
        }

        private static void AssertTransactionProperties(Transaction transaction, Guid guid, int accountId, int categoryId)
        {
            Assert.NotNull(transaction);
            Assert.Equal($"Test Transaction {guid}", transaction.Description);
            Assert.True(transaction.Id > 0);
            Assert.Equal(accountId, transaction.AccountId);
            Assert.Equal(categoryId, transaction.CategoryId);
            Assert.Equal(-10000, transaction.AmountCents);
            Assert.False(transaction.Paid);
            Assert.True(transaction.CreatedAt <= DateTime.UtcNow);
            Assert.True(transaction.UpdatedAt <= DateTime.UtcNow);
        }

        private static TransactionCreateOptions BuildTransactionCreateOptions(Guid guid, int accountId, int categoryId) => new TransactionCreateOptions
        {
            Description = $"Test Transaction {guid}",
            Date = DateTime.UtcNow,
            AmountCents = -10000,
            AccountId = accountId,
            CategoryId = categoryId,
            Paid = false
        };

        private static Account GetOrCreateTestAccount(NOrganizzeClient client, Guid guid)
        {
            return client.Accounts.Create(new AccountCreateOptions
            {
                Name = $"Test Account {guid}",
                Type = AccountType.Checking,
                Description = "Temporary test account"
            });
        }

        private static async Task<Account> GetOrCreateTestAccountAsync(NOrganizzeClient client, Guid guid, RequestOptions requestOptions, System.Threading.CancellationToken cancellationToken)
        {
            return await client.Accounts.CreateAsync(new AccountCreateOptions
            {
                Name = $"Test Account {guid}",
                Type = AccountType.Checking,
                Description = "Temporary test account"
            }, requestOptions, cancellationToken);
        }

        private static Category GetOrCreateTestCategory(NOrganizzeClient client, Guid guid)
        {
            return client.Categories.Create(new CategoryCreateOptions
            {
                Name = $"Test Category {guid}"
            });
        }

        private static async Task<Category> GetOrCreateTestCategoryAsync(NOrganizzeClient client, Guid guid, RequestOptions requestOptions, System.Threading.CancellationToken cancellationToken)
        {
            return await client.Categories.CreateAsync(new CategoryCreateOptions
            {
                Name = $"Test Category {guid}"
            }, requestOptions, cancellationToken);
        }

        private static void CleanupTestData(NOrganizzeClient client, int accountId, int categoryId)
        {
            client.Accounts.Delete(accountId);
            client.Categories.Delete(categoryId);
        }

        private static async Task CleanupTestDataAsync(NOrganizzeClient client, int accountId, int categoryId, RequestOptions requestOptions, System.Threading.CancellationToken cancellationToken)
        {
            await client.Accounts.DeleteAsync(accountId, requestOptions, cancellationToken);
            await client.Categories.DeleteAsync(categoryId, null, requestOptions, cancellationToken);
        }
    }
}
