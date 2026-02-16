using System;
using System.Collections.Generic;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using System.Text.Json.Serialization;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transactions
{
    public class TransactionUpdateOptions
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date")]
#if NET8_0_OR_GREATER
        [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
#else
        [Newtonsoft.Json.JsonConverter(typeof(DateOnlyJsonConverter))]
#endif
        public DateTime? Date { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("amount_cents")]
        public int? AmountCents { get; set; }

        [JsonProperty("category_id")]
        public long? CategoryId { get; set; }

        [JsonProperty("paid")]
        public bool? Paid { get; set; }

        [JsonProperty("update_future")]
        public bool? UpdateFuture { get; set; }

        [JsonProperty("update_all")]
        public bool? UpdateAll { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}
