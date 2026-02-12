using NOrganizze.CreditCards;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NOrganizze.Tests.CreditCards
{
    public class CreditCardServiceTests
    {
        private readonly NOrganizzeClientFixture _fixture;
        private readonly ITestContextAccessor _testContextAccessor;

        public CreditCardServiceTests(NOrganizzeClientFixture fixture, ITestContextAccessor testContextAccessor)
        {
            _fixture = fixture;
            _testContextAccessor = testContextAccessor;
        }

        [Fact]
        public void Test_CreditCardService_CrudOperations_Sync()
        {
            var guid = Guid.NewGuid();
            var client = _fixture.Client;

            var creditCard = client.CreditCards.Create(BuildCreditCardCreateOptions(guid));
            AssertCreditCardProperties(creditCard, guid);

            creditCard = client.CreditCards.Get(creditCard.Id);
            AssertCreditCardProperties(creditCard, guid);

            creditCard = client.CreditCards.Update(creditCard.Id, new CreditCardUpdateOptions
            {
                Name = $"Test Credit Card {guid}",
                DueDay = 15,
                ClosingDay = 10
            });

            var creditCards = client.CreditCards.List();
            creditCard = creditCards.Single(m => m.Id == creditCard.Id);
            Assert.Equal($"Test Credit Card {guid}", creditCard.Name);
            Assert.Equal(15, creditCard.DueDay);
            Assert.Equal(10, creditCard.ClosingDay);

            creditCard = client.CreditCards.Delete(creditCard.Id);
            Assert.Equal($"Test Credit Card {guid}", creditCard.Name);

            creditCards = client.CreditCards.List();
            Assert.DoesNotContain(creditCards, m => m.Id == creditCard.Id);
        }

        [Fact]
        public async Task Test_CreditCardService_CrudOperations_Async()
        {
            var guid = Guid.NewGuid();
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;

            var creditCard = await client.CreditCards.CreateAsync(BuildCreditCardCreateOptions(guid), requestOptions, cancellationToken);
            AssertCreditCardProperties(creditCard, guid);

            creditCard = await client.CreditCards.GetAsync(creditCard.Id, requestOptions, cancellationToken);
            AssertCreditCardProperties(creditCard, guid);

            creditCard = await client.CreditCards.UpdateAsync(creditCard.Id, new CreditCardUpdateOptions
            {
                Name = $"Test Credit Card {guid}",
                DueDay = 15,
                ClosingDay = 10
            }, requestOptions, cancellationToken);

            var creditCards = await client.CreditCards.ListAsync(requestOptions, cancellationToken);
            creditCard = creditCards.Single(m => m.Id == creditCard.Id);
            Assert.Equal($"Test Credit Card {guid}", creditCard.Name);
            Assert.Equal(15, creditCard.DueDay);
            Assert.Equal(10, creditCard.ClosingDay);

            creditCard = await client.CreditCards.DeleteAsync(creditCard.Id, requestOptions, cancellationToken);
            Assert.Equal($"Test Credit Card {guid}", creditCard.Name);

            creditCards = await client.CreditCards.ListAsync(requestOptions, cancellationToken);
            Assert.DoesNotContain(creditCards, m => m.Id == creditCard.Id);
        }

        [Fact]
        public void Test_CreditCardService_WithCardNetwork_Sync()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var client = _fixture.Client;
            var options = BuildCreditCardCreateOptions(guid);
            options.CardNetwork = CardNetwork.Visa;

            // Act
            var creditCard = client.CreditCards.Create(options);

            // Assert
            AssertCreditCardProperties(creditCard, guid);
            Assert.Equal(CardNetwork.Visa, creditCard.CardNetwork);

            // Clean up
            client.CreditCards.Delete(creditCard.Id);
        }

        [Fact]
        public async Task Test_CreditCardService_WithCardNetwork_Async()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;
            var options = BuildCreditCardCreateOptions(guid);
            options.CardNetwork = CardNetwork.Mastercard;

            // Act
            var creditCard = await client.CreditCards.CreateAsync(options, requestOptions, cancellationToken);

            // Assert
            AssertCreditCardProperties(creditCard, guid);
            Assert.Equal(CardNetwork.Mastercard, creditCard.CardNetwork);

            // Clean up
            await client.CreditCards.DeleteAsync(creditCard.Id, requestOptions, cancellationToken);
        }

        private static void AssertCreditCardProperties(CreditCard creditCard, Guid guid)
        {
            Assert.NotNull(creditCard);
            Assert.Equal($"Test Credit Card {guid}", creditCard.Name);
            Assert.True(creditCard.Id > 0);
            Assert.Equal(5, creditCard.DueDay);
            Assert.Equal(1, creditCard.ClosingDay);
            Assert.Equal("credit_card", creditCard.Kind);
            Assert.False(creditCard.Archived);
            Assert.True(creditCard.CreatedAt <= DateTime.UtcNow);
            Assert.True(creditCard.UpdatedAt <= DateTime.UtcNow);
        }

        private static CreditCardCreateOptions BuildCreditCardCreateOptions(Guid guid) => new CreditCardCreateOptions
        {
            Name = $"Test Credit Card {guid}",
            DueDay = 5,
            ClosingDay = 1,
            LimitCents = 100000
        };
    }
}
