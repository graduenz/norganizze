using NOrganizze.Invoices;
using System;
using System.Linq;
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

            if (!creditCards.Any())
            {
                // Skip test if no credit cards exist
                return;
            }

            var creditCardId = creditCards.First().Id;

            // Act
            var invoices = client.Invoices.List(creditCardId);

            // Assert
            Assert.NotNull(invoices);
            if (invoices.Any())
            {
                var invoice = invoices.First();
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

            if (!creditCards.Any())
            {
                return;
            }

            var creditCardId = creditCards.First().Id;

            // Act
            var invoices = await client.Invoices.ListAsync(creditCardId, null, requestOptions, cancellationToken);

            // Assert
            Assert.NotNull(invoices);
            if (invoices.Any())
            {
                var invoice = invoices.First();
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

            if (!creditCards.Any())
            {
                return;
            }

            var creditCardId = creditCards.First().Id;
            var options = new InvoiceListOptions
            {
                StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                EndDate = new DateTime(DateTime.Now.Year, 12, 31)
            };

            // Act
            var invoices = client.Invoices.List(creditCardId, options);

            // Assert
            Assert.NotNull(invoices);
            if (invoices.Any())
            {
                foreach (var invoice in invoices)
                {
                    Assert.Equal(DateTime.Now.Year, invoice.Date.Year);
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

            if (!creditCards.Any())
            {
                return;
            }

            var creditCardId = creditCards.First().Id;
            var options = new InvoiceListOptions
            {
                StartDate = new DateTime(DateTime.Now.Year, 1, 1),
                EndDate = new DateTime(DateTime.Now.Year, 12, 31)
            };

            // Act
            var invoices = await client.Invoices.ListAsync(creditCardId, options, requestOptions, cancellationToken);

            // Assert
            Assert.NotNull(invoices);
            if (invoices.Any())
            {
                foreach (var invoice in invoices)
                {
                    Assert.Equal(DateTime.Now.Year, invoice.Date.Year);
                }
            }
        }

        [Fact]
        public void Test_InvoiceService_Get_Sync()
        {
            // Arrange
            var client = _fixture.Client;
            var creditCards = client.CreditCards.List();

            if (!creditCards.Any())
            {
                return;
            }

            var creditCardId = creditCards.First().Id;
            var invoices = client.Invoices.List(creditCardId);

            if (!invoices.Any())
            {
                return;
            }

            var invoiceId = invoices.First().Id;

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

            if (!creditCards.Any())
            {
                return;
            }

            var creditCardId = creditCards.First().Id;
            var invoices = await client.Invoices.ListAsync(creditCardId, null, requestOptions, cancellationToken);

            if (!invoices.Any())
            {
                return;
            }

            var invoiceId = invoices.First().Id;

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
