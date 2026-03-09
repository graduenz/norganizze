#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Categories
{
    /// <summary>Options for creating a category. Pass to <see cref="CategoryService.Create"/> or <see cref="CategoryService.CreateAsync"/>.</summary>
    public class CategoryCreateOptions
    {
        /// <summary>Category name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
