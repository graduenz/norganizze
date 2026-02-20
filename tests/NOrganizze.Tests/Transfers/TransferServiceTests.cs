using NOrganizze.Accounts;
using NOrganizze.Transactions;
using NOrganizze.Transfers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NOrganizze.Tests.Transfers
{
    public class TransferServiceTests
    {
        private const string TemporaryTestAccountDescription = "Temporary test account";
        private const string UpdatedTransfer = "Updated transfer";
        private const string UpdatedNotes = "Updated notes";

        private readonly NOrganizzeClientFixture _fixture;
        private readonly ITestContextAccessor _testContextAccessor;

        public TransferServiceTests(NOrganizzeClientFixture fixture, ITestContextAccessor testContextAccessor)
        {
            _fixture = fixture;
            _testContextAccessor = testContextAccessor;
        }

        [Fact]
        public void Test_TransferService_CrudOperations_Sync()
        {
            var guid = Guid.NewGuid();
            var client = _fixture.Client;

            var fromAccount = GetOrCreateTestAccount(client, guid, "From");
            var toAccount = GetOrCreateTestAccount(client, guid, "To");

            var transfer = client.Transfers.Create(BuildTransferCreateOptions(fromAccount.Id, toAccount.Id));
            AssertTransferProperties(transfer);

            transfer = client.Transfers.Get(transfer.Id);
            AssertTransferProperties(transfer);

            transfer = client.Transfers.Update(transfer.Id, new TransferUpdateOptions
            {
                Description = $"{UpdatedTransfer} {guid}",
                Notes = UpdatedNotes
            });

            var transfers = client.Transfers.List();
            transfer = transfers.Single(m => m.Id == transfer.Id);
            Assert.Equal($"{UpdatedTransfer} {guid}", transfer.Description);
            Assert.Equal(UpdatedNotes, transfer.Notes);

            transfer = client.Transfers.Delete(transfer.Id);
            Assert.NotNull(transfer);

            transfers = client.Transfers.List();
            Assert.DoesNotContain(transfers, m => m.Id == transfer.Id);

            // Clean up
            CleanupTestData(client, fromAccount.Id, toAccount.Id);
        }

        [Fact]
        public async Task Test_TransferService_CrudOperations_Async()
        {
            var guid = Guid.NewGuid();
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;

            var fromAccount = await GetOrCreateTestAccountAsync(client, guid, "From", requestOptions, cancellationToken);
            var toAccount = await GetOrCreateTestAccountAsync(client, guid, "To", requestOptions, cancellationToken);

            var transfer = await client.Transfers.CreateAsync(BuildTransferCreateOptions(fromAccount.Id, toAccount.Id), requestOptions, cancellationToken);
            AssertTransferProperties(transfer);

            transfer = await client.Transfers.GetAsync(transfer.Id, requestOptions, cancellationToken);
            AssertTransferProperties(transfer);

            transfer = await client.Transfers.UpdateAsync(transfer.Id, new TransferUpdateOptions
            {
                Description = $"{UpdatedTransfer} {guid}",
                Notes = UpdatedNotes
            }, requestOptions, cancellationToken);

            var transfers = await client.Transfers.ListAsync(requestOptions, cancellationToken);
            transfer = transfers.Single(m => m.Id == transfer.Id);
            Assert.Equal($"{UpdatedTransfer} {guid}", transfer.Description);
            Assert.Equal(UpdatedNotes, transfer.Notes);

            transfer = await client.Transfers.DeleteAsync(transfer.Id, requestOptions, cancellationToken);
            Assert.NotNull(transfer);

            transfers = await client.Transfers.ListAsync(requestOptions, cancellationToken);
            Assert.DoesNotContain(transfers, m => m.Id == transfer.Id);

            // Clean up
            await CleanupTestDataAsync(client, fromAccount.Id, toAccount.Id, requestOptions, cancellationToken);
        }

        [Fact]
        public void Test_TransferService_WithTags_Sync()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var client = _fixture.Client;
            var fromAccount = GetOrCreateTestAccount(client, guid, "From");
            var toAccount = GetOrCreateTestAccount(client, guid, "To");
            var options = BuildTransferCreateOptions(fromAccount.Id, toAccount.Id);
            options.Tags = new System.Collections.Generic.List<Tag>
            {
                new Tag { Name = "transfer-test" },
                new Tag { Name = "automation" }
            };

            // Act
            var transfer = client.Transfers.Create(options);

            // Assert
            Assert.NotNull(transfer.Tags);
            Assert.True(transfer.Tags.Count >= 2);

            // Clean up
            client.Transfers.Delete(transfer.Id);
            CleanupTestData(client, fromAccount.Id, toAccount.Id);
        }

        [Fact]
        public async Task Test_TransferService_WithTags_Async()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;
            var fromAccount = await GetOrCreateTestAccountAsync(client, guid, "From", requestOptions, cancellationToken);
            var toAccount = await GetOrCreateTestAccountAsync(client, guid, "To", requestOptions, cancellationToken);
            var options = BuildTransferCreateOptions(fromAccount.Id, toAccount.Id);
            options.Tags = new System.Collections.Generic.List<Tag>
            {
                new Tag { Name = "transfer-test" },
                new Tag { Name = "automation" }
            };

            // Act
            var transfer = await client.Transfers.CreateAsync(options, requestOptions, cancellationToken);

            // Assert
            Assert.NotNull(transfer.Tags);
            Assert.True(transfer.Tags.Count >= 2);

            // Clean up
            await client.Transfers.DeleteAsync(transfer.Id, requestOptions, cancellationToken);
            await CleanupTestDataAsync(client, fromAccount.Id, toAccount.Id, requestOptions, cancellationToken);
        }

        private static void AssertTransferProperties(Transfer transfer)
        {
            Assert.NotNull(transfer);
            Assert.True(transfer.Id > 0);
            Assert.Equal(-5000, transfer.AmountCents);
            Assert.False(transfer.Paid);
            Assert.True(transfer.CreatedAt <= DateTime.UtcNow);
            Assert.True(transfer.UpdatedAt <= DateTime.UtcNow);
        }

        private static TransferCreateOptions BuildTransferCreateOptions(long fromAccountId, long toAccountId) => new TransferCreateOptions
        {
            CreditAccountId = fromAccountId,
            DebitAccountId = toAccountId,
            AmountCents = 5000,
            Date = DateTime.UtcNow,
            Paid = false
        };

        private static Account GetOrCreateTestAccount(NOrganizzeClient client, Guid guid, string suffix)
        {
            return client.Accounts.Create(new AccountCreateOptions
            {
                Name = $"Test Account {guid} {suffix}",
                Type = AccountType.Checking,
                Description = TemporaryTestAccountDescription
            });
        }

        private static async Task<Account> GetOrCreateTestAccountAsync(NOrganizzeClient client, Guid guid, string suffix, RequestOptions requestOptions, System.Threading.CancellationToken cancellationToken)
        {
            return await client.Accounts.CreateAsync(new AccountCreateOptions
            {
                Name = $"Test Account {guid} {suffix}",
                Type = AccountType.Checking,
                Description = TemporaryTestAccountDescription
            }, requestOptions, cancellationToken);
        }

        private static void CleanupTestData(NOrganizzeClient client, long fromAccountId, long toAccountId)
        {
            client.Accounts.Delete(fromAccountId);
            client.Accounts.Delete(toAccountId);
        }

        private static async Task CleanupTestDataAsync(NOrganizzeClient client, long fromAccountId, long toAccountId, RequestOptions requestOptions, System.Threading.CancellationToken cancellationToken)
        {
            await client.Accounts.DeleteAsync(fromAccountId, requestOptions, cancellationToken);
            await client.Accounts.DeleteAsync(toAccountId, requestOptions, cancellationToken);
        }
    }
}
