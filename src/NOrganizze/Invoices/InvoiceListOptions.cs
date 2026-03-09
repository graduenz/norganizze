using System;

namespace NOrganizze.Invoices
{
    /// <summary>Options to filter the invoice list. Pass to <see cref="InvoiceService.List"/> or <see cref="InvoiceService.ListAsync"/>.</summary>
    public class InvoiceListOptions
    {
        /// <summary>Inclusive start date for the range. Sent to the API as yyyy-MM-dd.</summary>
        public DateTime? StartDate { get; set; }
        /// <summary>Inclusive end date for the range. Sent to the API as yyyy-MM-dd.</summary>
        public DateTime? EndDate { get; set; }
    }
}
