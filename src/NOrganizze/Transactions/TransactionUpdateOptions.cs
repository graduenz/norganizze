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
    /// <summary>Options for updating a transaction. Pass to <see cref="TransactionService.Update"/> or <see cref="TransactionService.UpdateAsync"/>.</summary>
    public class TransactionUpdateOptions
    {
        /// <summary>Updated description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Updated date. Serialized as date-only (yyyy-MM-dd).</summary>
        [JsonProperty("date")]
#if NET8_0_OR_GREATER
        [JsonConverter(typeof(NullableDateOnlyJsonConverter))]
#else
        [Newtonsoft.Json.JsonConverter(typeof(DateOnlyJsonConverter))]
#endif
        public DateTime? Date { get; set; }

        /// <summary>Updated notes.</summary>
        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>Updated amount in cents.</summary>
        [JsonProperty("amount_cents")]
        public int? AmountCents { get; set; }

        /// <summary>Updated category id.</summary>
        [JsonProperty("category_id")]
        public long? CategoryId { get; set; }

        /// <summary>Updated paid status.</summary>
        [JsonProperty("paid")]
        public bool? Paid { get; set; }

        /// <summary>When updating a recurring transaction, whether to update future occurrences.</summary>
        [JsonProperty("update_future")]
        public bool? UpdateFuture { get; set; }

        /// <summary>When updating a recurring transaction, whether to update all occurrences.</summary>
        [JsonProperty("update_all")]
        public bool? UpdateAll { get; set; }

        /// <summary>Updated tags.</summary>
        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}
