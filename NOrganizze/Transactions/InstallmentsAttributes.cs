#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transactions
{
    public class InstallmentsAttributes
    {
        [JsonProperty("periodicity")]
        public string Periodicity { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
