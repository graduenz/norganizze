using System;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using System.Text.Json.Serialization;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.CreditCards
{
    public class CreditCardUpdateOptions
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("due_day")]
        public int? DueDay { get; set; }

        [JsonProperty("closing_day")]
        public int? ClosingDay { get; set; }

        [JsonProperty("update_invoices_since")]
#if NET8_0_OR_GREATER
        [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
#else
        [Newtonsoft.Json.JsonConverter(typeof(DateOnlyJsonConverter))]
#endif
        public DateTime? UpdateInvoicesSince { get; set; }
    }
}
