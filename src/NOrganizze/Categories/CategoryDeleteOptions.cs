#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Categories
{
    /// <summary>Options for deleting a category. Pass to <see cref="CategoryService.Delete"/> or <see cref="CategoryService.DeleteAsync"/> when the API requires a replacement category for existing transactions.</summary>
    public class CategoryDeleteOptions
    {
        /// <summary>Id of the category to reassign transactions to when deleting this category.</summary>
        [JsonProperty("replacement_id")]
        public long? ReplacementId { get; set; }
    }
}
