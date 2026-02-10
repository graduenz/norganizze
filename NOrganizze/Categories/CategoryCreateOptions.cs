#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Categories
{
    public class CategoryCreateOptions
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
