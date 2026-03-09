using System;
using System.Collections.Generic;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transactions
{
    /// <summary>Represents a transaction (expense or income) returned by the Organizze API.</summary>
    public class Transaction
    {
        /// <summary>Transaction id.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>Description.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Transaction date.</summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>Whether the transaction is marked as paid.</summary>
        [JsonProperty("paid")]
        public bool Paid { get; set; }

        /// <summary>Amount in cents.</summary>
        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        /// <summary>Total number of installments (for installment transactions).</summary>
        [JsonProperty("total_installments")]
        public int TotalInstallments { get; set; }

        /// <summary>Current installment number.</summary>
        [JsonProperty("installment")]
        public int Installment { get; set; }

        /// <summary>Whether the transaction is recurring.</summary>
        [JsonProperty("recurring")]
        public bool Recurring { get; set; }

        /// <summary>Account id.</summary>
        [JsonProperty("account_id")]
        public long AccountId { get; set; }

        /// <summary>Account type (e.g. checking, credit_card).</summary>
        [JsonProperty("account_type")]
        public string AccountType { get; set; }

        /// <summary>Category id.</summary>
        [JsonProperty("category_id")]
        public long CategoryId { get; set; }

        /// <summary>Optional contact id.</summary>
        [JsonProperty("contact_id")]
        public long? ContactId { get; set; }

        /// <summary>Optional notes.</summary>
        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>Number of attachments.</summary>
        [JsonProperty("attachments_count")]
        public int AttachmentsCount { get; set; }

        /// <summary>Credit card id when the transaction is from a credit card.</summary>
        [JsonProperty("credit_card_id")]
        public long? CreditCardId { get; set; }

        /// <summary>Credit card invoice id.</summary>
        [JsonProperty("credit_card_invoice_id")]
        public long? CreditCardInvoiceId { get; set; }

        /// <summary>Paid credit card id.</summary>
        [JsonProperty("paid_credit_card_id")]
        public long? PaidCreditCardId { get; set; }

        /// <summary>Paid credit card invoice id.</summary>
        [JsonProperty("paid_credit_card_invoice_id")]
        public long? PaidCreditCardInvoiceId { get; set; }

        /// <summary>Opposite transaction id (e.g. for transfers).</summary>
        [JsonProperty("oposite_transaction_id")]
        public long? OppositeTransactionId { get; set; }

        /// <summary>Opposite account id.</summary>
        [JsonProperty("oposite_account_id")]
        public long? OppositeAccountId { get; set; }

        /// <summary>Creation timestamp.</summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>Last update timestamp.</summary>
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>Tags associated with the transaction.</summary>
        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}
