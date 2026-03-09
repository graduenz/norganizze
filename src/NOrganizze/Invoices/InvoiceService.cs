using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NOrganizze.Transactions;

namespace NOrganizze.Invoices
{
    /// <summary>Service for listing and inspecting credit card invoices. Use <see cref="InvoiceListOptions"/> with <see cref="List"/> or <see cref="ListAsync"/> to filter by date range.</summary>
    public class InvoiceService : Service
    {
        private const string CreditCards = "credit_cards";
        private const string Invoices = "invoices";
        private const string Payments = "payments";

        /// <summary>Initializes a new instance of the <see cref="InvoiceService"/> class.</summary>
        public InvoiceService(NOrganizzeClient client) : base(client)
        {
        }

        /// <summary>Lists invoices for a credit card, optionally filtered by date range. Pass <paramref name="options"/> with <see cref="InvoiceListOptions.StartDate"/> and/or <see cref="InvoiceListOptions.EndDate"/>.</summary>
        /// <param name="creditCardId">Credit card id.</param>
        /// <param name="options">Optional. Use <see cref="InvoiceListOptions"/> to filter by start and end date. Dates sent as yyyy-MM-dd.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>List of invoices.</returns>
        public List<Invoice> List(long creditCardId, InvoiceListOptions options = null, RequestOptions requestOptions = null)
        {
            var path = BuildListPath(creditCardId, options);
            return Get<List<Invoice>>(path, requestOptions);
        }

        /// <summary>Lists invoices for a credit card asynchronously, optionally filtered by date range.</summary>
        /// <param name="creditCardId">Credit card id.</param>
        /// <param name="options">Optional. Use <see cref="InvoiceListOptions"/> to filter by start and end date.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List of invoices.</returns>
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

        /// <summary>Gets invoice details for a specific invoice of a credit card.</summary>
        /// <param name="creditCardId">Credit card id.</param>
        /// <param name="invoiceId">Invoice id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>Invoice details.</returns>
        public InvoiceDetail Get(long creditCardId, long invoiceId, RequestOptions requestOptions = null)
        {
            return Get<InvoiceDetail>($"{CreditCards}/{creditCardId}/{Invoices}/{invoiceId}", requestOptions);
        }

        /// <summary>Gets invoice details asynchronously.</summary>
        /// <param name="creditCardId">Credit card id.</param>
        /// <param name="invoiceId">Invoice id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Invoice details.</returns>
        public Task<InvoiceDetail> GetAsync(long creditCardId, long invoiceId, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<InvoiceDetail>($"{CreditCards}/{creditCardId}/{Invoices}/{invoiceId}", requestOptions, cancellationToken);
        }

        /// <summary>Gets the payment transaction for an invoice.</summary>
        /// <param name="creditCardId">Credit card id.</param>
        /// <param name="invoiceId">Invoice id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <returns>The payment transaction.</returns>
        public Transaction GetPayment(long creditCardId, long invoiceId, RequestOptions requestOptions = null)
        {
            return Get<Transaction>($"{CreditCards}/{creditCardId}/{Invoices}/{invoiceId}/{Payments}", requestOptions);
        }

        /// <summary>Gets the payment transaction for an invoice asynchronously.</summary>
        /// <param name="creditCardId">Credit card id.</param>
        /// <param name="invoiceId">Invoice id.</param>
        /// <param name="requestOptions">Optional per-request overrides.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The payment transaction.</returns>
        public Task<Transaction> GetPaymentAsync(long creditCardId, long invoiceId, RequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<Transaction>($"{CreditCards}/{creditCardId}/{Invoices}/{invoiceId}/{Payments}", requestOptions, cancellationToken);
        }
    }
}
