using NOrganizze.Invoices;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NOrganizze.Tests.Invoices
{
    public class InvoiceServiceTests
    {
        private readonly NOrganizzeClientFixture _fixture;
        private readonly ITestContextAccessor _testContextAccessor;

        public InvoiceServiceTests(NOrganizzeClientFixture fixture, ITestContextAccessor testContextAccessor)
        {
            _fixture = fixture;
            _testContextAccessor = testContextAccessor;
        }

        [Fact]
        public void Test_InvoiceService_List_Sync()
        {
            // Arrange
            var client = _fixture.Client;
            var creditCards = client.CreditCards.List();

            if (creditCards.Count == 0)
            {
                // Skip test if no credit cards exist
                return;
            }

            var creditCardId = creditCards[0].Id;

            // Act
            var invoices = client.Invoices.List(creditCardId);

            // Assert
            Assert.NotNull(invoices);
            if (invoices.Count > 0)
            {
                var invoice = invoices[0];
                AssertInvoiceProperties(invoice);
                Assert.Equal(creditCardId, invoice.CreditCardId);
            }
        }

        [Fact]
        public async Task Test_InvoiceService_List_Async()
        {
            // Arrange
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;
            var creditCards = await client.CreditCards.ListAsync(requestOptions, cancellationToken);

            if (creditCards.Count == 0)
            {
                return;
            }

            var creditCardId = creditCards[0].Id;

            // Act
            var invoices = await client.Invoices.ListAsync(creditCardId, null, requestOptions, cancellationToken);

            // Assert
            Assert.NotNull(invoices);
            if (invoices.Count > 0)
            {
                var invoice = invoices[0];
                AssertInvoiceProperties(invoice);
                Assert.Equal(creditCardId, invoice.CreditCardId);
            }
        }

        [Fact]
        public void Test_InvoiceService_ListWithDateFilter_Sync()
        {
            // Arrange
            var client = _fixture.Client;
            var creditCards = client.CreditCards.List();

            if (creditCards.Count == 0)
            {
                return;
            }

            var creditCardId = creditCards[0].Id;
            var currentYear = DateTime.UtcNow.Year;
            var options = new InvoiceListOptions
            {
                StartDate = new DateTime(currentYear, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(currentYear, 12, 31, 23, 59, 59, DateTimeKind.Utc)
            };

            // Act
            var invoices = client.Invoices.List(creditCardId, options);

            // Assert
            Assert.NotNull(invoices);
            if (invoices.Count > 0)
            {
                foreach (var invoice in invoices)
                {
                    Assert.Equal(currentYear, invoice.Date.Year);
                }
            }
        }

        [Fact]
        public async Task Test_InvoiceService_ListWithDateFilter_Async()
        {
            // Arrange
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;
            var creditCards = await client.CreditCards.ListAsync(requestOptions, cancellationToken);

            if (creditCards.Count == 0)
            {
                return;
            }

            var creditCardId = creditCards[0].Id;
            var currentYear = DateTime.UtcNow.Year;
            var options = new InvoiceListOptions
            {
                StartDate = new DateTime(currentYear, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(currentYear, 12, 31, 23, 59, 59, DateTimeKind.Utc)
            };

            // Act
            var invoices = await client.Invoices.ListAsync(creditCardId, options, requestOptions, cancellationToken);

            // Assert
            Assert.NotNull(invoices);
            if (invoices.Count > 0)
            {
                foreach (var invoice in invoices)
                {
                    Assert.Equal(currentYear, invoice.Date.Year);
                }
            }
        }

        [Fact]
        public void Test_InvoiceService_Get_Sync()
        {
            // Arrange
            var client = _fixture.Client;
            var creditCards = client.CreditCards.List();

            if (creditCards.Count == 0)
            {
                return;
            }

            var creditCardId = creditCards[0].Id;
            var invoices = client.Invoices.List(creditCardId);

            if (invoices.Count == 0)
            {
                return;
            }

            var invoiceId = invoices[0].Id;

            // Act
            var invoiceDetail = client.Invoices.Get(creditCardId, invoiceId);

            // Assert
            Assert.NotNull(invoiceDetail);
            Assert.Equal(invoiceId, invoiceDetail.Id);
            Assert.Equal(creditCardId, invoiceDetail.CreditCardId);
            Assert.NotNull(invoiceDetail.Transactions);
            Assert.NotNull(invoiceDetail.Payments);
        }

        [Fact]
        public async Task Test_InvoiceService_Get_Async()
        {
            // Arrange
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;
            var creditCards = await client.CreditCards.ListAsync(requestOptions, cancellationToken);

            if (creditCards.Count == 0)
            {
                return;
            }

            var creditCardId = creditCards[0].Id;
            var invoices = await client.Invoices.ListAsync(creditCardId, null, requestOptions, cancellationToken);

            if (invoices.Count == 0)
            {
                return;
            }

            var invoiceId = invoices[0].Id;

            // Act
            var invoiceDetail = await client.Invoices.GetAsync(creditCardId, invoiceId, requestOptions, cancellationToken);

            // Assert
            Assert.NotNull(invoiceDetail);
            Assert.Equal(invoiceId, invoiceDetail.Id);
            Assert.Equal(creditCardId, invoiceDetail.CreditCardId);
            Assert.NotNull(invoiceDetail.Transactions);
            Assert.NotNull(invoiceDetail.Payments);
        }

        private static void AssertInvoiceProperties(Invoice invoice)
        {
            Assert.NotNull(invoice);
            Assert.True(invoice.Id > 0);
            Assert.True(invoice.CreditCardId > 0);
            Assert.True(invoice.Date != default);
            Assert.True(invoice.StartingDate != default);
            Assert.True(invoice.ClosingDate != default);
        }
    }
}
