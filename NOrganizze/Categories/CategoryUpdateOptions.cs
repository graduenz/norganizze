#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Categories
{
    public class CategoryUpdateOptions
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }
}
