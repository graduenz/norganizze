using System;
using System.Collections.Generic;
#if NET8_0_OR_GREATER
using JsonPropertyAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;
#else
using JsonPropertyAttribute = Newtonsoft.Json.JsonPropertyAttribute;
#endif

namespace NOrganizze.Transactions
{
    public class Transaction
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("paid")]
        public bool Paid { get; set; }

        [JsonProperty("amount_cents")]
        public int AmountCents { get; set; }

        [JsonProperty("total_installments")]
        public int TotalInstallments { get; set; }

        [JsonProperty("installment")]
        public int Installment { get; set; }

        [JsonProperty("recurring")]
        public bool Recurring { get; set; }

        [JsonProperty("account_id")]
        public int AccountId { get; set; }

        [JsonProperty("account_type")]
        public string AccountType { get; set; }

        [JsonProperty("category_id")]
        public int CategoryId { get; set; }

        [JsonProperty("contact_id")]
        public int? ContactId { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("attachments_count")]
        public int AttachmentsCount { get; set; }

        [JsonProperty("credit_card_id")]
        public int? CreditCardId { get; set; }

        [JsonProperty("credit_card_invoice_id")]
        public int? CreditCardInvoiceId { get; set; }

        [JsonProperty("paid_credit_card_id")]
        public int? PaidCreditCardId { get; set; }

        [JsonProperty("paid_credit_card_invoice_id")]
        public int? PaidCreditCardInvoiceId { get; set; }

        [JsonProperty("oposite_transaction_id")]
        public int? OpositeTransactionId { get; set; }

        [JsonProperty("oposite_account_id")]
        public int? OpositeAccountId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}
