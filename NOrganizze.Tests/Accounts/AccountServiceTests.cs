using NOrganizze.Accounts;
using System;
using System.Linq;
using Xunit;

namespace NOrganizze.Tests.Accounts
{
    public class AccountServiceTests
    {
        private readonly NOrganizzeClientFixture _fixture;

        public AccountServiceTests(NOrganizzeClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Test_AccountService_Sync()
        {
            var guid = Guid.NewGuid();

            var account = _fixture.Client.Accounts.Create(BuildAccountCreateOptions(guid, "new account"));
            AssertAccountProperties(account, guid, "new account");

            account = _fixture.Client.Accounts.Get(account.Id);
            AssertAccountProperties(account, guid, "new account");

            account = _fixture.Client.Accounts.Update(account.Id, new AccountUpdateOptions
            {
                Name = $"Test Account {guid}",
                Description = "updated account",
                Default = false
            });

            var accounts = _fixture.Client.Accounts.List();
            Assert.NotEmpty(accounts);
            account = accounts.Single(m => m.Id == account.Id);
            AssertAccountProperties(account, guid, "updated account");

            account = _fixture.Client.Accounts.Delete(account.Id);
            AssertAccountProperties(account, guid, "updated account");

            accounts = _fixture.Client.Accounts.List();
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
