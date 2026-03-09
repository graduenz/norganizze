#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.CreditCards
{
    /// <summary>Options for creating a credit card. Pass to <see cref="CreditCardService.Create"/> or <see cref="CreditCardService.CreateAsync"/>.</summary>
    public class CreditCardCreateOptions
    {
        /// <summary>Credit card name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Card network (e.g. Visa, Mastercard).</summary>
        [JsonProperty("card_network")]
        public string CardNetwork { get; set; }

        /// <summary>Day of month when the invoice is due.</summary>
        [JsonProperty("due_day")]
        public int DueDay { get; set; }

        /// <summary>Day of month when the invoice closes.</summary>
        [JsonProperty("closing_day")]
        public int ClosingDay { get; set; }

        /// <summary>Optional credit limit in cents.</summary>
        [JsonProperty("limit_cents")]
        public int? LimitCents { get; set; }
    }
}
