using System;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Invoices
{
    /// <summary>Represents a credit card invoice returned by the Organizze API.</summary>
    public class Invoice
    {
        /// <summary>Invoice id.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Invoice date.</summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>Starting date of the invoice period.</summary>
        [JsonProperty("starting_date")]
        public DateTime StartingDate { get; set; }

        /// <summary>Closing date of the invoice period.</summary>
        [JsonProperty("closing_date")]
        public DateTime ClosingDate { get; set; }

        /// <summary>Invoice amount in cents.</summary>
        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        /// <summary>Payment amount in cents.</summary>
        [JsonProperty("payment_amount_cents")]
        public int PaymentAmountCents { get; set; }

        /// <summary>Balance in cents.</summary>
        [JsonProperty("balance_cents")]
        public int BalanceCents { get; set; }

        /// <summary>Previous balance in cents.</summary>
        [JsonProperty("previous_balance_cents")]
        public int PreviousBalanceCents { get; set; }

        /// <summary>Credit card id.</summary>
        [JsonProperty("credit_card_id")]
        public long CreditCardId { get; set; }
    }
}
