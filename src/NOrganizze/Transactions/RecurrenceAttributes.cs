#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transactions
{
    /// <summary>Recurrence configuration for a transaction. Use with <see cref="TransactionCreateOptions.RecurrenceAttributes"/>.</summary>
    public class RecurrenceAttributes
    {
        /// <summary>Recurrence period (e.g. <see cref="Periodicity.Monthly"/>, <see cref="Periodicity.Weekly"/>).</summary>
        [JsonProperty("periodicity")]
        public string Periodicity { get; set; }
    }
}
