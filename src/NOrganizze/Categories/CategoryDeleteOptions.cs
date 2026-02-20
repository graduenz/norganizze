#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Categories
{
    public class CategoryDeleteOptions
    {
        [JsonProperty("replacement_id")]
        public long? ReplacementId { get; set; }
    }
}
