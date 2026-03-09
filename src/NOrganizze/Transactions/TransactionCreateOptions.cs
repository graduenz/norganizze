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
    /// <summary>Options for creating a transaction. Pass to <see cref="TransactionService.Create"/> or <see cref="TransactionService.CreateAsync"/>.</summary>
    public class TransactionCreateOptions
    {
        /// <summary>Transaction description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Transaction date. Serialized as date-only (yyyy-MM-dd).</summary>
        [JsonProperty("date")]
#if NET8_0_OR_GREATER
        [JsonConverter(typeof(DateOnlyJsonConverter))]
#else
        [Newtonsoft.Json.JsonConverter(typeof(DateOnlyJsonConverter))]
#endif
        public DateTime Date { get; set; }

        /// <summary>Optional notes.</summary>
        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>Amount in cents.</summary>
        [JsonProperty("amount_cents")]
        public int? AmountCents { get; set; }

        /// <summary>Category id.</summary>
        [JsonProperty("category_id")]
        public long? CategoryId { get; set; }

        /// <summary>Account id.</summary>
        [JsonProperty("account_id")]
        public long? AccountId { get; set; }

        /// <summary>Whether the transaction is marked as paid.</summary>
        [JsonProperty("paid")]
        public bool? Paid { get; set; }

        /// <summary>Tags to associate with the transaction.</summary>
        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }

        /// <summary>Recurrence configuration.</summary>
        [JsonProperty("recurrence_attributes")]
        public RecurrenceAttributes RecurrenceAttributes { get; set; }

        /// <summary>Installments configuration.</summary>
        [JsonProperty("installments_attributes")]
        public InstallmentsAttributes InstallmentsAttributes { get; set; }
    }
}
