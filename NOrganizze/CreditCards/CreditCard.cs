using System;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.CreditCards
{
    public class CreditCard
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("card_network")]
        public string CardNetwork { get; set; }

        [JsonProperty("closing_day")]
        public int ClosingDay { get; set; }

        [JsonProperty("due_day")]
        public int DueDay { get; set; }

        [JsonProperty("limit_cents")]
        public int LimitCents { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("default")]
        public bool Default { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
