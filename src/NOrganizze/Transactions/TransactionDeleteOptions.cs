#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transactions
{
    /// <summary>Optional parameters for deleting a transaction. Pass to <see cref="TransactionService.Delete"/> or <see cref="TransactionService.DeleteAsync"/> when the API supports them.</summary>
    public class TransactionDeleteOptions
    {
        /// <summary>When deleting a recurring transaction, whether to delete future occurrences.</summary>
        [JsonProperty("update_future")]
        public bool? UpdateFuture { get; set; }

        /// <summary>When deleting a recurring transaction, whether to delete all occurrences.</summary>
        [JsonProperty("update_all")]
        public bool? UpdateAll { get; set; }
    }
}
