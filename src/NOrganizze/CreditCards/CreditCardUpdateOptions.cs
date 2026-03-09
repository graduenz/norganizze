using System;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using System.Text.Json.Serialization;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.CreditCards
{
    /// <summary>Options for updating a credit card. Pass to <see cref="CreditCardService.Update"/> or <see cref="CreditCardService.UpdateAsync"/>.</summary>
    public class CreditCardUpdateOptions
    {
        /// <summary>Updated name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Updated due day of month.</summary>
        [JsonProperty("due_day")]
        public int? DueDay { get; set; }

        /// <summary>Updated closing day of month.</summary>
        [JsonProperty("closing_day")]
        public int? ClosingDay { get; set; }

        /// <summary>Optional date from which to update invoices (date-only, serialized as yyyy-MM-dd).</summary>
        [JsonProperty("update_invoices_since")]
#if NET8_0_OR_GREATER
        [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
#else
        [Newtonsoft.Json.JsonConverter(typeof(DateOnlyJsonConverter))]
#endif
        public DateTime? UpdateInvoicesSince { get; set; }
    }
}
