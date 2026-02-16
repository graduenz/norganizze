using System;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Budgets
{
    public class Budget
    {
        [JsonProperty("amount_in_cents")]
        public int AmountInCents { get; set; }

        [JsonProperty("category_id")]
        public long CategoryId { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("activity_type")]
        public int ActivityType { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("predicted_total")]
        public int PredictedTotal { get; set; }

        [JsonProperty("percentage")]
        public string Percentage { get; set; }
    }
}
