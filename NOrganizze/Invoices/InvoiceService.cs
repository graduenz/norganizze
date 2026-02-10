using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NOrganizze.Transactions;

namespace NOrganizze.Invoices
{
    public class InvoiceService : Service
    {
        public InvoiceService(NOrganizzeClient client) : base(client)
        {
        }

        public List<Invoice> List(int creditCardId, InvoiceListOptions options = null, RequestOptions requestOptions = null)
        {
            var path = BuildListPath(creditCardId, options);
            return Get<List<Invoice>>(path, requestOptions);
        }

        public Task<List<Invoice>> ListAsync(int creditCardId, InvoiceListOptions options = null, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            var path = BuildListPath(creditCardId, options);
            return GetAsync<List<Invoice>>(path, requestOptions, cancellationToken);
        }

        private static string BuildListPath(int creditCardId, InvoiceListOptions options)
        {
            var path = $"credit_cards/{creditCardId}/invoices";
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

        public InvoiceDetail Get(int creditCardId, int invoiceId, RequestOptions requestOptions = null)
        {
            return Get<InvoiceDetail>($"credit_cards/{creditCardId}/invoices/{invoiceId}", requestOptions);
        }

        public Task<InvoiceDetail> GetAsync(int creditCardId, int invoiceId, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<InvoiceDetail>($"credit_cards/{creditCardId}/invoices/{invoiceId}", requestOptions, cancellationToken);
        }

        public Transaction GetPayment(int creditCardId, int invoiceId, RequestOptions requestOptions = null)
        {
            return Get<Transaction>($"credit_cards/{creditCardId}/invoices/{invoiceId}/payments", requestOptions);
        }

        public Task<Transaction> GetPaymentAsync(int creditCardId, int invoiceId, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Transaction>($"credit_cards/{creditCardId}/invoices/{invoiceId}/payments", requestOptions, cancellationToken);
        }
    }
}
