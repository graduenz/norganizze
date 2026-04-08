using Bogus;
using Moq;
using Moq.Protected;
using NOrganizze.Transactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NOrganizze.Tests.Transactions
{
    public class TransactionServiceUnitTests
    {
        private const string TestEmail = "tests@example.com";
        private const string TestApiKey = "test-api-key";

        [Fact]
        public void List_AutoPaginateFalse_PerformsSingleRequest()
        {
            // Arrange
            var pages = new[]
            {
                BuildBatch(TransactionService.MaxTransactionsPerRequest, new DateTime(2026, 4, 1), 1),
                BuildBatch(10, new DateTime(2026, 4, 2), 10000)
            };
            var fixture = CreateServiceFixture(pages);
            var options = new TransactionListOptions
            {
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 4, 30),
                AutoPaginate = false
            };

            // Act
            var result = fixture.Service.List(options);

            // Assert
            Assert.Equal(TransactionService.MaxTransactionsPerRequest, result.Count);
            Assert.Single(fixture.RequestUris);
        }

        [Fact]
        public async Task ListAsync_AutoPaginateFalse_PerformsSingleRequest()
        {
            // Arrange
            var pages = new[]
            {
                BuildBatch(TransactionService.MaxTransactionsPerRequest, new DateTime(2026, 4, 1), 1),
                BuildBatch(10, new DateTime(2026, 4, 2), 10000)
            };
            var fixture = CreateServiceFixture(pages);
            var options = new TransactionListOptions
            {
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 4, 30),
                AutoPaginate = false
            };

            // Act
            var result = await fixture.Service.ListAsync(options, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(TransactionService.MaxTransactionsPerRequest, result.Count);
            Assert.Single(fixture.RequestUris);
        }

        [Fact]
        public void List_NullOptions_AutoPaginationRunsWithDefaults()
        {
            // Arrange
            var fixture = CreateServiceFixture(new[] { BuildBatch(10, DateTime.UtcNow.Date, 1) });

            // Act
            var result = fixture.Service.List(null);

            // Assert
            Assert.Equal(10, result.Count);
            Assert.Single(fixture.RequestUris);
        }

        [Fact]
        public async Task ListAsync_NullOptions_AutoPaginationRunsWithDefaults()
        {
            // Arrange
            var fixture = CreateServiceFixture(new[] { BuildBatch(10, DateTime.UtcNow.Date, 1) });

            // Act
            var result = await fixture.Service.ListAsync(null, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(10, result.Count);
            Assert.Single(fixture.RequestUris);
        }

        [Fact]
        public void List_MissingStartDate_UsesCurrentMonthStart()
        {
            // Arrange
            var fixture = CreateServiceFixture(new[] { BuildBatch(5, DateTime.UtcNow.Date, 1) });
            var options = new TransactionListOptions
            {
                EndDate = new DateTime(2026, 4, 30),
                AutoPaginate = true
            };

            // Act
            fixture.Service.List(options);

            // Assert
            var now = DateTime.UtcNow;
            var expectedStart = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            Assert.Equal(expectedStart, GetQueryValue(fixture.RequestUris.Single(), "start_date"));
        }

        [Fact]
        public void List_MissingEndDate_UsesCurrentMonthEnd()
        {
            // Arrange
            var fixture = CreateServiceFixture(new[] { BuildBatch(5, DateTime.UtcNow.Date, 1) });
            var options = new TransactionListOptions
            {
                StartDate = new DateTime(2026, 4, 1),
                AutoPaginate = true
            };

            // Act
            fixture.Service.List(options);

            // Assert
            var now = DateTime.UtcNow;
            var expectedEnd = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month))
                .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            Assert.Equal(expectedEnd, GetQueryValue(fixture.RequestUris.Single(), "end_date"));
        }

        [Fact]
        public async Task ListAsync_MissingDates_UsesCurrentMonthBoundaries()
        {
            // Arrange
            var fixture = CreateServiceFixture(new[] { BuildBatch(5, DateTime.UtcNow.Date, 1) });
            var options = new TransactionListOptions { AutoPaginate = true };

            // Act
            await fixture.Service.ListAsync(options, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            var now = DateTime.UtcNow;
            var expectedStart = new DateTime(now.Year, now.Month, 1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var expectedEnd = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month))
                .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var uri = fixture.RequestUris.Single();
            Assert.Equal(expectedStart, GetQueryValue(uri, "start_date"));
            Assert.Equal(expectedEnd, GetQueryValue(uri, "end_date"));
        }

        [Fact]
        public void List_FirstBatchAtLimitAndProgresses_FetchesNextPage()
        {
            // Arrange
            var first = BuildBatch(TransactionService.MaxTransactionsPerRequest, new DateTime(2026, 4, 2), 1);
            var second = BuildBatch(10, new DateTime(2026, 4, 3), 10000);
            var fixture = CreateServiceFixture(new[] { first, second });

            // Act
            var result = fixture.Service.List(new TransactionListOptions
            {
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 4, 30)
            });

            // Assert
            Assert.Equal(TransactionService.MaxTransactionsPerRequest + 10, result.Count);
            Assert.Equal(2, fixture.RequestUris.Count);
        }

        [Fact]
        public void List_FirstBatchBelowLimit_StopsAfterFirstPage()
        {
            // Arrange
            var fixture = CreateServiceFixture(new[] { BuildBatch(10, new DateTime(2026, 4, 1), 1) });

            // Act
            var result = fixture.Service.List(new TransactionListOptions
            {
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 4, 30)
            });

            // Assert
            Assert.Equal(10, result.Count);
            Assert.Single(fixture.RequestUris);
        }

        [Fact]
        public void List_MaxDateNotGreaterThanCursor_StopsToAvoidInfiniteLoop()
        {
            // Arrange
            var fixedDate = new DateTime(2026, 4, 1);
            var first = BuildBatch(TransactionService.MaxTransactionsPerRequest, fixedDate, 1);
            var fixture = CreateServiceFixture(new[] { first });

            // Act
            var result = fixture.Service.List(new TransactionListOptions
            {
                StartDate = fixedDate,
                EndDate = new DateTime(2026, 4, 30)
            });

            // Assert
            Assert.Equal(TransactionService.MaxTransactionsPerRequest, result.Count);
            Assert.Single(fixture.RequestUris);
        }

        [Fact]
        public void List_NoNewItemsAfterDedup_StopsToAvoidInfiniteLoop()
        {
            // Arrange
            var first = BuildBatch(TransactionService.MaxTransactionsPerRequest, new DateTime(2026, 4, 2), 1);
            var second = CloneBatchWithDate(first, new DateTime(2026, 4, 3));
            var fixture = CreateServiceFixture(new[] { first, second });

            // Act
            var result = fixture.Service.List(new TransactionListOptions
            {
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 4, 30)
            });

            // Assert
            Assert.Equal(TransactionService.MaxTransactionsPerRequest, result.Count);
            Assert.Equal(2, fixture.RequestUris.Count);
        }

        [Fact]
        public async Task ListAsync_FirstBatchAtLimitAndProgresses_FetchesNextPage()
        {
            // Arrange
            var first = BuildBatch(TransactionService.MaxTransactionsPerRequest, new DateTime(2026, 4, 2), 1);
            var second = BuildBatch(10, new DateTime(2026, 4, 3), 10000);
            var fixture = CreateServiceFixture(new[] { first, second });

            // Act
            var result = await fixture.Service.ListAsync(new TransactionListOptions
            {
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 4, 30)
            }, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(TransactionService.MaxTransactionsPerRequest + 10, result.Count);
            Assert.Equal(2, fixture.RequestUris.Count);
        }

        [Fact]
        public async Task ListAsync_FirstBatchBelowLimit_StopsAfterFirstPage()
        {
            // Arrange
            var fixture = CreateServiceFixture(new[] { BuildBatch(10, new DateTime(2026, 4, 1), 1) });

            // Act
            var result = await fixture.Service.ListAsync(new TransactionListOptions
            {
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 4, 30)
            }, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(10, result.Count);
            Assert.Single(fixture.RequestUris);
        }

        [Fact]
        public void List_OverlappingBatches_DeduplicatesById()
        {
            // Arrange
            var first = BuildBatch(TransactionService.MaxTransactionsPerRequest, new DateTime(2026, 4, 2), 1);
            var overlap = first.Take(100).Select(x => new Transaction { Id = x.Id, Date = x.Date }).ToList();
            var second = overlap.Concat(BuildBatch(10, new DateTime(2026, 4, 3), 50000)).ToList();
            var fixture = CreateServiceFixture(new[] { first, second });

            // Act
            var result = fixture.Service.List(new TransactionListOptions
            {
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 4, 30)
            });

            // Assert
            Assert.Equal(TransactionService.MaxTransactionsPerRequest + 10, result.Count);
        }

        [Fact]
        public async Task ListAsync_OverlappingBatches_DeduplicatesById()
        {
            // Arrange
            var first = BuildBatch(TransactionService.MaxTransactionsPerRequest, new DateTime(2026, 4, 2), 1);
            var overlap = first.Take(100).Select(x => new Transaction { Id = x.Id, Date = x.Date }).ToList();
            var second = overlap.Concat(BuildBatch(10, new DateTime(2026, 4, 3), 50000)).ToList();
            var fixture = CreateServiceFixture(new[] { first, second });

            // Act
            var result = await fixture.Service.ListAsync(new TransactionListOptions
            {
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 4, 30)
            }, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(TransactionService.MaxTransactionsPerRequest + 10, result.Count);
        }

        [Fact]
        public void List_WithAccountId_SendsAccountIdInEveryPagedRequest()
        {
            // Arrange
            var fixture = CreateServiceFixture(new[]
            {
                BuildBatch(TransactionService.MaxTransactionsPerRequest, new DateTime(2026, 4, 2), 1),
                BuildBatch(10, new DateTime(2026, 4, 3), 10000)
            });

            // Act
            fixture.Service.List(new TransactionListOptions
            {
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 4, 30),
                AccountId = 12345
            });

            // Assert
            Assert.Equal(2, fixture.RequestUris.Count);
            Assert.All(fixture.RequestUris, uri => Assert.Equal("12345", GetQueryValue(uri, "account_id")));
        }

        [Fact]
        public async Task ListAsync_WithAccountId_SendsAccountIdInEveryPagedRequest()
        {
            // Arrange
            var fixture = CreateServiceFixture(new[]
            {
                BuildBatch(TransactionService.MaxTransactionsPerRequest, new DateTime(2026, 4, 2), 1),
                BuildBatch(10, new DateTime(2026, 4, 3), 10000)
            });

            // Act
            await fixture.Service.ListAsync(new TransactionListOptions
            {
                StartDate = new DateTime(2026, 4, 1),
                EndDate = new DateTime(2026, 4, 30),
                AccountId = 12345
            }, cancellationToken: TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(2, fixture.RequestUris.Count);
            Assert.All(fixture.RequestUris, uri => Assert.Equal("12345", GetQueryValue(uri, "account_id")));
        }

        private static (TransactionService Service, List<Uri> RequestUris) CreateServiceFixture(IEnumerable<List<Transaction>> pages)
        {
            var requestUris = new List<Uri>();
            var pageQueue = new Queue<List<Transaction>>(pages);

            var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            Func<HttpRequestMessage, HttpResponseMessage> nextResponse = request =>
            {
                requestUris.Add(request.RequestUri);
                if (pageQueue.Count == 0)
                    throw new InvalidOperationException("Unexpected extra HTTP request.");
                return CreateJsonResponse(pageQueue.Dequeue());
            };

            handler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((request, _) => Task.FromResult(nextResponse(request)));

#if NET8_0_OR_GREATER
            handler
                .Protected()
                .Setup<HttpResponseMessage>(
                    "Send",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((request, _) => nextResponse(request));
#endif

            var httpClient = new HttpClient(handler.Object);
            var client = new NOrganizzeClient(httpClient, TestEmail, TestApiKey);
            return (client.Transactions, requestUris);
        }

        private static HttpResponseMessage CreateJsonResponse(List<Transaction> batch)
        {
#if NET8_0_OR_GREATER
            var json = System.Text.Json.JsonSerializer.Serialize(batch);
#else
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(batch);
#endif
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        private static List<Transaction> BuildBatch(int count, DateTime date, long startId)
        {
            var id = startId;
            var faker = new Faker<Transaction>()
                .RuleFor(x => x.Id, _ => id++)
                .RuleFor(x => x.Date, _ => date)
                .RuleFor(x => x.Description, f => f.Commerce.ProductName())
                .RuleFor(x => x.Paid, f => f.Random.Bool())
                .RuleFor(x => x.AmountCents, f => f.Random.Int(-10000, 10000))
                .RuleFor(x => x.TotalInstallments, _ => 1)
                .RuleFor(x => x.Installment, _ => 1)
                .RuleFor(x => x.Recurring, _ => false)
                .RuleFor(x => x.AccountId, _ => 1)
                .RuleFor(x => x.AccountType, _ => "Account")
                .RuleFor(x => x.CategoryId, _ => 1)
                .RuleFor(x => x.CreatedAt, _ => date)
                .RuleFor(x => x.UpdatedAt, _ => date);

            return faker.Generate(count);
        }

        private static List<Transaction> CloneBatchWithDate(IEnumerable<Transaction> source, DateTime date)
        {
            return source.Select(x => new Transaction
            {
                Id = x.Id,
                Date = date,
                Description = x.Description,
                Paid = x.Paid,
                AmountCents = x.AmountCents,
                TotalInstallments = x.TotalInstallments,
                Installment = x.Installment,
                Recurring = x.Recurring,
                AccountId = x.AccountId,
                AccountType = x.AccountType,
                CategoryId = x.CategoryId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();
        }

        private static string GetQueryValue(Uri uri, string key)
        {
            var query = uri.Query;
            if (query.StartsWith("?", StringComparison.Ordinal))
                query = query.Substring(1);

            foreach (var pair in query.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var pieces = pair.Split(new[] { '=' }, 2);
                if (pieces.Length == 2 && string.Equals(pieces[0], key, StringComparison.Ordinal))
                    return Uri.UnescapeDataString(pieces[1]);
            }

            return null;
        }
    }
}
