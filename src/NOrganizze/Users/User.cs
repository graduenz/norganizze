#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Users
{
    /// <summary>Represents a user returned by the Organizze API.</summary>
    public class User
    {
        /// <summary>User id.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>User name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>User email.</summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>User role.</summary>
        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
