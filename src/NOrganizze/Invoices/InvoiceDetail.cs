using System.Collections.Generic;
using NOrganizze.Transactions;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Invoices
{
    public class InvoiceDetail : Invoice
    {
        [JsonProperty("transactions")]
        public List<Transaction> Transactions { get; set; }

        [JsonProperty("payments")]
        public List<Transaction> Payments { get; set; }
    }
}
