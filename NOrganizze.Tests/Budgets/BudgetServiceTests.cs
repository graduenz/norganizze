using NOrganizze.Budgets;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NOrganizze.Tests.Budgets
{
    public class BudgetServiceTests
    {
        private readonly NOrganizzeClientFixture _fixture;
        private readonly ITestContextAccessor _testContextAccessor;

        public BudgetServiceTests(NOrganizzeClientFixture fixture, ITestContextAccessor testContextAccessor)
        {
            _fixture = fixture;
            _testContextAccessor = testContextAccessor;
        }

        [Fact]
        public void Test_BudgetService_List_Sync()
        {
            // Arrange
            var client = _fixture.Client;

            // Act
            var budgets = client.Budgets.List();

            // Assert
            Assert.NotNull(budgets);
            if (budgets.Count > 0)
            {
                var budget = budgets.First();
                AssertBudgetProperties(budget);
            }
        }

        [Fact]
        public async Task Test_BudgetService_List_Async()
        {
            // Arrange
            var client = _fixture.Client;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;

            // Act
            var budgets = await client.Budgets.ListAsync(requestOptions, cancellationToken);

            // Assert
            Assert.NotNull(budgets);
            if (budgets.Count > 0)
            {
                var budget = budgets.First();
                AssertBudgetProperties(budget);
            }
        }

        [Fact]
        public void Test_BudgetService_ListByYear_Sync()
        {
            // Arrange
            var client = _fixture.Client;
            var year = DateTime.Now.Year;

            // Act
            var budgets = client.Budgets.ListByYear(year);

            // Assert
            Assert.NotNull(budgets);
            if (budgets.Count > 0)
            {
                var budget = budgets.First();
                AssertBudgetProperties(budget);
                Assert.Equal(year, budget.Date.Year);
            }
        }

        [Fact]
        public async Task Test_BudgetService_ListByYear_Async()
        {
            // Arrange
            var client = _fixture.Client;
            var year = DateTime.Now.Year;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;

            // Act
            var budgets = await client.Budgets.ListByYearAsync(year, requestOptions, cancellationToken);

            // Assert
            Assert.NotNull(budgets);
            if (budgets.Count > 0)
            {
                var budget = budgets.First();
                AssertBudgetProperties(budget);
                Assert.Equal(year, budget.Date.Year);
            }
        }

        [Fact]
        public void Test_BudgetService_ListByMonth_Sync()
        {
            // Arrange
            var client = _fixture.Client;
            var now = DateTime.Now;
            var year = now.Year;
            var month = now.Month;

            // Act
            var budgets = client.Budgets.ListByMonth(year, month);

            // Assert
            Assert.NotNull(budgets);
            if (budgets.Count > 0)
            {
                var budget = budgets.First();
                AssertBudgetProperties(budget);
                Assert.Equal(year, budget.Date.Year);
                Assert.Equal(month, budget.Date.Month);
            }
        }

        [Fact]
        public async Task Test_BudgetService_ListByMonth_Async()
        {
            // Arrange
            var client = _fixture.Client;
            var now = DateTime.Now;
            var year = now.Year;
            var month = now.Month;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;

            // Act
            var budgets = await client.Budgets.ListByMonthAsync(year, month, requestOptions, cancellationToken);

            // Assert
            Assert.NotNull(budgets);
            if (budgets.Count > 0)
            {
                var budget = budgets.First();
                AssertBudgetProperties(budget);
                Assert.Equal(year, budget.Date.Year);
                Assert.Equal(month, budget.Date.Month);
            }
        }

        [Fact]
        public void Test_BudgetService_ListByMonth_PreviousMonth_Sync()
        {
            // Arrange
            var client = _fixture.Client;
            var previousMonth = DateTime.Now.AddMonths(-1);
            var year = previousMonth.Year;
            var month = previousMonth.Month;

            // Act
            var budgets = client.Budgets.ListByMonth(year, month);

            // Assert
            Assert.NotNull(budgets);
            if (budgets.Count > 0)
            {
                var budget = budgets.First();
                AssertBudgetProperties(budget);
                Assert.Equal(year, budget.Date.Year);
                Assert.Equal(month, budget.Date.Month);
            }
        }

        [Fact]
        public async Task Test_BudgetService_ListByMonth_PreviousMonth_Async()
        {
            // Arrange
            var client = _fixture.Client;
            var previousMonth = DateTime.Now.AddMonths(-1);
            var year = previousMonth.Year;
            var month = previousMonth.Month;
            RequestOptions requestOptions = null;
            var cancellationToken = _testContextAccessor.Current.CancellationToken;

            // Act
            var budgets = await client.Budgets.ListByMonthAsync(year, month, requestOptions, cancellationToken);

            // Assert
            Assert.NotNull(budgets);
            if (budgets.Count > 0)
            {
                var budget = budgets.First();
                AssertBudgetProperties(budget);
                Assert.Equal(year, budget.Date.Year);
                Assert.Equal(month, budget.Date.Month);
            }
        }

        private static void AssertBudgetProperties(Budget budget)
        {
            Assert.NotNull(budget);
            Assert.True(budget.CategoryId > 0);
            Assert.True(budget.Date != default);
            Assert.NotNull(budget.Percentage);
        }
    }
}
