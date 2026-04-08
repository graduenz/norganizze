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

        /// <summary>
        /// When true (the default), the library automatically fetches all transactions beyond
        /// the Organizze API limit of <see cref="TransactionService.MaxTransactionsPerRequest"/>
        /// per request. It does so by advancing <see cref="StartDate"/> to the latest transaction
        /// date from each batch and repeating until fewer than
        /// <see cref="TransactionService.MaxTransactionsPerRequest"/> results are returned,
        /// deduplicating by transaction id. When <see cref="StartDate"/> or <see cref="EndDate"/>
        /// are not set, the library defaults to the current month boundaries (UTC).
        /// Set to false to make a single API call (original behavior).
        /// </summary>
        public bool? AutoPaginate { get; set; }
    }
}
