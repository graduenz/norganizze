#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transactions
{
    /// <summary>Represents a tag that can be associated with transactions.</summary>
    public class Tag
    {
        /// <summary>Tag name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
