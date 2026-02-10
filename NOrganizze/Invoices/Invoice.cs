using System;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Invoices
{
    public class Invoice
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("starting_date")]
        public DateTime StartingDate { get; set; }

        [JsonProperty("closing_date")]
        public DateTime ClosingDate { get; set; }

        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        [JsonProperty("payment_amount_cents")]
        public int PaymentAmountCents { get; set; }

        [JsonProperty("balance_cents")]
        public int BalanceCents { get; set; }

        [JsonProperty("previous_balance_cents")]
        public int PreviousBalanceCents { get; set; }

        [JsonProperty("credit_card_id")]
        public int CreditCardId { get; set; }
    }
}
