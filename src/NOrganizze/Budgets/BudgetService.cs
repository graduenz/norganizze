using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NOrganizze.Budgets
{
    public class BudgetService : Service
    {
        private const string Budgets = "budgets";

        public BudgetService(NOrganizzeClient client) : base(client)
        {
        }

        public List<Budget> List(RequestOptions requestOptions = null)
        {
            return Get<List<Budget>>(Budgets, requestOptions);
        }

        public Task<List<Budget>> ListAsync(RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Budget>>(Budgets, requestOptions, cancellationToken);
        }

        public List<Budget> ListByYear(int year, RequestOptions requestOptions = null)
        {
            return Get<List<Budget>>($"{Budgets}/{year}", requestOptions);
        }

        public Task<List<Budget>> ListByYearAsync(int year, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Budget>>($"{Budgets}/{year}", requestOptions, cancellationToken);
        }

        public List<Budget> ListByMonth(int year, int month, RequestOptions requestOptions = null)
        {
            return Get<List<Budget>>($"{Budgets}/{year}/{month}", requestOptions);
        }

        public Task<List<Budget>> ListByMonthAsync(int year, int month, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<List<Budget>>($"{Budgets}/{year}/{month}", requestOptions, cancellationToken);
        }
    }
}
