using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Budgets
{
    /// <summary>Service for listing budgets. Use <see cref="List"/> for all budgets, <see cref="ListByYear"/> for a specific year, or <see cref="ListByMonth"/> for a specific year and month.</summary>
    public class BudgetService : Service
    {
        private const string Budgets = "budgets";

        /// <summary>Initializes a new instance of the <see cref="BudgetService"/> class.</summary>
        public BudgetService(NOrganizzeClient client) : base(client)
        {
        }

        /// <summary>Lists all budgets.</summary>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>List of budgets.</returns>
        public List<Budget> List(RequestOptions requestOptions = null)
        {
            return Get<List<Budget>>(Budgets, requestOptions);
        }

        /// <summary>Lists all budgets asynchronously.</summary>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of budgets.</returns>
        public Task<List<Budget>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Budget>>(Budgets, requestOptions, cancellationToken);
        }

        /// <summary>Lists budgets for a specific year.</summary>
        /// <param name="year">Year (e.g. 2025).</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>List of budgets for the year.</returns>
        public List<Budget> ListByYear(int year, RequestOptions requestOptions = null)
        {
            return Get<List<Budget>>($"{Budgets}/{year}", requestOptions);
        }

        /// <summary>Lists budgets for a specific year asynchronously.</summary>
        /// <param name="year">Year (e.g. 2025).</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of budgets for the year.</returns>
        public Task<List<Budget>> ListByYearAsync(int year, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Budget>>($"{Budgets}/{year}", requestOptions, cancellationToken);
        }

        /// <summary>Lists budgets for a specific year and month.</summary>
        /// <param name="year">Year (e.g. 2025).</param>
        /// <param name="month">Month (1–12).</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>List of budgets for the year and month.</returns>
        public List<Budget> ListByMonth(int year, int month, RequestOptions requestOptions = null)
        {
            return Get<List<Budget>>($"{Budgets}/{year}/{month}", requestOptions);
        }

        /// <summary>Lists budgets for a specific year and month asynchronously.</summary>
        /// <param name="year">Year (e.g. 2025).</param>
        /// <param name="month">Month (1–12).</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of budgets for the year and month.</returns>
        public Task<List<Budget>> ListByMonthAsync(int year, int month, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Budget>>($"{Budgets}/{year}/{month}", requestOptions, cancellationToken);
        }
    }
}
