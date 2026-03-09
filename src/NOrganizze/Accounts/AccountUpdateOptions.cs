#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Accounts
{
    /// <summary>Options for updating an account. Pass to <see cref="AccountService.Update"/> or <see cref="AccountService.UpdateAsync"/>.</summary>
    public class AccountUpdateOptions
    {
        /// <summary>Updated name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Updated description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Whether this account is the default.</summary>
        [JsonProperty("default")]
        public bool? Default { get; set; }
    }
}
