using System.Collections.Generic;
using NOrganizze.Transactions;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transfers
{
    public class TransferUpdateOptions
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}
