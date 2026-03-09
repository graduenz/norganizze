#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transactions
{
    /// <summary>Installments configuration for a transaction. Use with <see cref="TransactionCreateOptions.InstallmentsAttributes"/>.</summary>
    public class InstallmentsAttributes
    {
        /// <summary>Installment periodicity (e.g. <see cref="Periodicity.Monthly"/>).</summary>
        [JsonProperty("periodicity")]
        public string Periodicity { get; set; }

        /// <summary>Total number of installments.</summary>
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
