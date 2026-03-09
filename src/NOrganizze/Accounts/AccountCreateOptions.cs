#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Accounts
{
    /// <summary>Options for creating an account. Pass to <see cref="AccountService.Create"/> or <see cref="AccountService.CreateAsync"/>.</summary>
    public class AccountCreateOptions
    {
        /// <summary>Account name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Account type (e.g. checking, savings).</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>Optional description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Whether this account is the default.</summary>
        [JsonProperty("default")]
        public bool? Default { get; set; }
    }
}
