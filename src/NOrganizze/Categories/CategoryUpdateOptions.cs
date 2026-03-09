#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Categories
{
    /// <summary>Options for updating a category. Pass to <see cref="CategoryService.Update"/> or <see cref="CategoryService.UpdateAsync"/>.</summary>
    public class CategoryUpdateOptions
    {
        /// <summary>Updated category name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

