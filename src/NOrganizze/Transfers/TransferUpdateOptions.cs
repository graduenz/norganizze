using System.Collections.Generic;
using NOrganizze.Transactions;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transfers
{
    /// <summary>Options for updating a transfer. Pass to <see cref="TransferService.Update"/> or <see cref="TransferService.UpdateAsync"/>.</summary>
    public class TransferUpdateOptions
    {
        /// <summary>Updated description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Updated notes.</summary>
        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>Updated tags.</summary>
        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}
