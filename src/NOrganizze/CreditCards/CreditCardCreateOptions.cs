#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.CreditCards
{
    public class CreditCardCreateOptions
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("card_network")]
        public string CardNetwork { get; set; }

        [JsonProperty("due_day")]
        public int DueDay { get; set; }

        [JsonProperty("closing_day")]
        public int ClosingDay { get; set; }

        [JsonProperty("limit_cents")]
        public int? LimitCents { get; set; }
    }
}
