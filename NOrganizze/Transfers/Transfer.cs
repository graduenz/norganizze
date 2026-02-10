using System.Collections.Generic;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transfers
{
    public class Transfer : Transactions.Transaction
    {
        [JsonProperty("recurrence_id")]
        public int? RecurrenceId { get; set; }

        [JsonProperty("attachments")]
        public List<object> Attachments { get; set; }
    }
}
