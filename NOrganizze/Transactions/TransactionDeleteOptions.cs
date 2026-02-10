#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transactions
{
    public class TransactionDeleteOptions
    {
        [JsonProperty("update_future")]
        public bool? UpdateFuture { get; set; }

        [JsonProperty("update_all")]
        public bool? UpdateAll { get; set; }
    }
}
