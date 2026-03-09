using System;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.CreditCards
{
    /// <summary>Represents a credit card returned by the Organizze API.</summary>
    public class CreditCard
    {
        /// <summary>Credit card id.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Credit card name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Optional description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Card network (e.g. Visa, Mastercard).</summary>
        [JsonProperty("card_network")]
        public string CardNetwork { get; set; }

        /// <summary>Closing day of month.</summary>
        [JsonProperty("closing_day")]
        public int ClosingDay { get; set; }

        /// <summary>Due day of month.</summary>
        [JsonProperty("due_day")]
        public int DueDay { get; set; }

        /// <summary>Credit limit in cents.</summary>
        [JsonProperty("limit_cents")]
        public int LimitCents { get; set; }

        /// <summary>Credit card type/kind.</summary>
        [JsonProperty("type")]
        public string Kind { get; set; }

        /// <summary>Whether the card is archived.</summary>
        [JsonProperty("archived")]
        public bool Archived { get; set; }

        /// <summary>Whether this card is the default.</summary>
        [JsonProperty("default")]
        public bool Default { get; set; }

        /// <summary>Creation timestamp.</summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>Last update timestamp.</summary>
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
