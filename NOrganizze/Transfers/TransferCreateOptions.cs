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
    public class TransferCreateOptions
    {
        [JsonProperty("credit_account_id")]
        public long CreditAccountId { get; set; }

        [JsonProperty("debit_account_id")]
        public long DebitAccountId { get; set; }

        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        [JsonProperty("date")]
#if NET8_0_OR_GREATER
        [JsonConverter(typeof(DateOnlyJsonConverter))]
#else
        [Newtonsoft.Json.JsonConverter(typeof(DateOnlyJsonConverter))]
#endif
        public DateTime Date { get; set; }

        [JsonProperty("paid")]
        public bool? Paid { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}
