#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Categories
{
    /// <summary>Represents a category returned by the Organizze API.</summary>
    public class Category
    {
        /// <summary>Category id.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Category name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Display color.</summary>
        [JsonProperty("color")]
        public string Color { get; set; }

        /// <summary>Parent category id for subcategories.</summary>
        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }
    }
}
