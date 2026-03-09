using System.Collections.Generic;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transfers
{
    /// <summary>Represents a transfer between accounts returned by the Organizze API.</summary>
    public class Transfer : Transactions.Transaction
    {
        /// <summary>Recurrence id when the transfer is part of a recurring series.</summary>
        [JsonProperty("recurrence_id")]
        public long? RecurrenceId { get; set; }

        /// <summary>Attachments associated with the transfer.</summary>
        [JsonProperty("attachments")]
        public List<object> Attachments { get; set; }
    }
}
