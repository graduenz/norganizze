using System;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Budgets
{
    /// <summary>Represents a budget entry returned by the Organizze API.</summary>
    public class Budget
    {
        /// <summary>Budget amount in cents.</summary>
        [JsonProperty("amount_in_cents")]
        public int AmountInCents { get; set; }

        /// <summary>Category id.</summary>
        [JsonProperty("category_id")]
        public long CategoryId { get; set; }

        /// <summary>Budget date.</summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>Activity type.</summary>
        [JsonProperty("activity_type")]
        public int ActivityType { get; set; }

        /// <summary>Total.</summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>Predicted total.</summary>
        [JsonProperty("predicted_total")]
        public int PredictedTotal { get; set; }

        /// <summary>Percentage (as string from API).</summary>
        [JsonProperty("percentage")]
        public string Percentage { get; set; }
    }
}
