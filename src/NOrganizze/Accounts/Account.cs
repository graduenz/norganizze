using System;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Accounts
{
    /// <summary>Represents an account (bank account or credit card) returned by the Organizze API.</summary>
    public class Account
    {
        /// <summary>Account id.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Account name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Optional description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Whether the account is archived.</summary>
        [JsonProperty("archived")]
        public bool Archived { get; set; }

        /// <summary>Creation timestamp.</summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>Last update timestamp.</summary>
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>Whether this account is the default.</summary>
        [JsonProperty("default")]
        public bool Default { get; set; }

        /// <summary>Account type (e.g. checking, savings).</summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
