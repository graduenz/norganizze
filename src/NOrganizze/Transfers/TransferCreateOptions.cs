using System;
using System.Collections.Generic;
using NOrganizze.Transactions;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
using System.Text.Json.Serialization;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transfers
{
    /// <summary>Options for creating a transfer. Pass to <see cref="TransferService.Create"/> or <see cref="TransferService.CreateAsync"/>.</summary>
    public class TransferCreateOptions
    {
        /// <summary>Id of the account that receives the transfer (credit).</summary>
        [JsonProperty("credit_account_id")]
        public long CreditAccountId { get; set; }

        /// <summary>Id of the account that sends the transfer (debit).</summary>
        [JsonProperty("debit_account_id")]
        public long DebitAccountId { get; set; }

        /// <summary>Amount in cents.</summary>
        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        /// <summary>Transfer date. Serialized as date-only (yyyy-MM-dd).</summary>
        [JsonProperty("date")]
#if NET8_0_OR_GREATER
        [JsonConverter(typeof(DateOnlyJsonConverter))]
#else
        [Newtonsoft.Json.JsonConverter(typeof(DateOnlyJsonConverter))]
#endif
        public DateTime Date { get; set; }

        /// <summary>Whether the transfer is marked as paid.</summary>
        [JsonProperty("paid")]
        public bool? Paid { get; set; }

        /// <summary>Optional tags.</summary>
        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}
