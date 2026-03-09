using System.Collections.Generic;
using NOrganizze.Transactions;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Invoices
{
    /// <summary>Invoice with full details including transactions and payments. Returned by <see cref="InvoiceService.Get"/> and <see cref="InvoiceService.GetAsync"/>.</summary>
    public class InvoiceDetail : Invoice
    {
        /// <summary>Transactions in this invoice.</summary>
        [JsonProperty("transactions")]
        public List<Transaction> Transactions { get; set; }

        /// <summary>Payments for this invoice.</summary>
        [JsonProperty("payments")]
        public List<Transaction> Payments { get; set; }
    }
}
