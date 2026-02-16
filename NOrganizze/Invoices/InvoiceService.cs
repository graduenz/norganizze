using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NOrganizze.Transactions;

namespace NOrganizze.Invoices
{
    public class InvoiceService : Service
    {
        private const string CreditCards = "credit_cards";
        private const string Invoices = "invoices";
        private const string Payments = "payments";

        public InvoiceService(NOrganizzeClient client) : base(client)
        {
        }

        public List<Invoice> List(long creditCardId, InvoiceListOptions options = null, RequestOptions requestOptions = null)
        {
            var path = BuildListPath(creditCardId, options);
            return Get<List<Invoice>>(path, requestOptions);
        }

        public Task<List<Invoice>> ListAsync(long creditCardId, InvoiceListOptions options = null, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            var path = BuildListPath(creditCardId, options);
            return GetAsync<List<Invoice>>(path, requestOptions, cancellationToken);
        }

        private static string BuildListPath(long creditCardId, InvoiceListOptions options)
        {
            var path = $"{CreditCards}/{creditCardId}/{Invoices}";
            if (options != null)
            {
                var queryParams = new List<string>();
                if (options.StartDate.HasValue)
                    queryParams.Add($"start_date={UrlEncode(options.StartDate.Value.ToString("yyyy-MM-dd"))}");
                if (options.EndDate.HasValue)
                    queryParams.Add($"end_date={UrlEncode(options.EndDate.Value.ToString("yyyy-MM-dd"))}");

                if (queryParams.Count > 0)
                    path += "?" + string.Join("&", queryParams);
            }

            return path;
        }

        public InvoiceDetail Get(long creditCardId, long invoiceId, RequestOptions requestOptions = null)
        {
            return Get<InvoiceDetail>($"{CreditCards}/{creditCardId}/{Invoices}/{invoiceId}", requestOptions);
        }

        public Task<InvoiceDetail> GetAsync(long creditCardId, long invoiceId, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<InvoiceDetail>($"{CreditCards}/{creditCardId}/{Invoices}/{invoiceId}", requestOptions, cancellationToken);
        }

        public Transaction GetPayment(long creditCardId, long invoiceId, RequestOptions requestOptions = null)
        {
            return Get<Transaction>($"{CreditCards}/{creditCardId}/{Invoices}/{invoiceId}/{Payments}", requestOptions);
        }

        public Task<Transaction> GetPaymentAsync(long creditCardId, long invoiceId, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Transaction>($"{CreditCards}/{creditCardId}/{Invoices}/{invoiceId}/{Payments}", requestOptions, cancellationToken);
        }
    }
}
