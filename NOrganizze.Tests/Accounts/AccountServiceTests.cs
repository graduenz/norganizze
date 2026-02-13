using NOrganizze.Accounts;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NOrganizze.Tests.Accounts
{
    public class AccountServiceTests
    {
        private const string NewAccountDescription = "New account";
        private const string UpdatedAccountDescription = "Updated account";

        private readonly NOrganizzeClientFixture _fixture;
        private readonly ITestContextAccessor _testContextAccessor;

        public AccountServiceTests(NOrganizzeClientFixture fixture, ITestContextAccessor testContextAccessor)
        {
            _fixture = fixture;
            _testContextAccessor = testContextAccessor;
        }

        [Fact]
        public void Test_AccountService_CrudOperations_Sync()
        {
            var guid = Guid.NewGuid();
            var client = _fixture.Client;

            var account = client.Accounts.Create(BuildAccountCreateOptions(guid, NewAccountDescription));
            AssertAccountProperties(account, guid, NewAccountDescription);

            account = client.Accounts.Get(account.Id);
            AssertAccountProperties(account, guid, NewAccountDescription);

            account = client.Accounts.Update(account.Id, new AccountUpdateOptions
            {
                Name = $"Test Account {guid}",
                Description = UpdatedAccountDescription,
                Default = false
            });

            var accounts = client.Accounts.List();
            account = accounts.Single(m => m.Id == account.Id);
            AssertAccountProperties(account, guid, UpdatedAccountDescription);

            account = client.Accounts.Delete(account.Id);
            AssertAccountProperties(account, guid, UpdatedAccountDescription);

            accounts = client.Accounts.List();
            Assert.DoesNotContain(accounts, m => m.Id == account.Id);
        }

        [Fact]
        public async Task Test_AccountService_CrudOperations_Async()
        {
            var guid = Guid.NewGuid();
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;

            var account = await client.Accounts.CreateAsync(BuildAccountCreateOptions(guid, NewAccountDescription), requestOptions, cancellationToken);
            AssertAccountProperties(account, guid, NewAccountDescription);

            account = await client.Accounts.GetAsync(account.Id, requestOptions, cancellationToken);
            AssertAccountProperties(account, guid, NewAccountDescription);

            account = await client.Accounts.UpdateAsync(account.Id, new AccountUpdateOptions
            {
                Name = $"Test Account {guid}",
                Description = UpdatedAccountDescription,
                Default = false
            }, requestOptions, cancellationToken);

            var accounts = await client.Accounts.ListAsync(requestOptions, cancellationToken);
            account = accounts.Single(m => m.Id == account.Id);
            AssertAccountProperties(account, guid, UpdatedAccountDescription);

            account = await client.Accounts.DeleteAsync(account.Id, requestOptions, cancellationToken);
            AssertAccountProperties(account, guid, UpdatedAccountDescription);

            accounts = await client.Accounts.ListAsync(requestOptions, cancellationToken);
            Assert.DoesNotContain(accounts, m => m.Id == account.Id);
        }

        private static void AssertAccountProperties(Account account, Guid guid, string description)
        {
            Assert.NotNull(account);
            Assert.Equal($"Test Account {guid}", account.Name);
            Assert.Equal(description, account.Description);
            Assert.False(account.Default);
            Assert.False(account.Archived);
            Assert.True(account.Id > 0);
            Assert.True(account.CreatedAt <= DateTime.UtcNow);
            Assert.True(account.UpdatedAt <= DateTime.UtcNow);
        }

        private static AccountCreateOptions BuildAccountCreateOptions(Guid guid, string description) => new AccountCreateOptions
        {
            Name = $"Test Account {guid}",
            Type = "checking",
            Description = description,
            Default = false
        };
    }
}
