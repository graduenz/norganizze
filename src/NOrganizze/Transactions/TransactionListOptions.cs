using System;

namespace NOrganizze.Transactions
{
    /// <summary>Options to filter the transaction list. Pass to <see cref="TransactionService.List"/> or <see cref="TransactionService.ListAsync"/>.</summary>
    public class TransactionListOptions
    {
        /// <summary>Inclusive start date for the range. Sent to the API as yyyy-MM-dd.</summary>
        public DateTime? StartDate { get; set; }
        /// <summary>Inclusive end date for the range. Sent to the API as yyyy-MM-dd.</summary>
        public DateTime? EndDate { get; set; }
        /// <summary>Filter by account id.</summary>
        public long? AccountId { get; set; }
    }
}
