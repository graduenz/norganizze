using System;

namespace NOrganizze.Transactions
{
    public class TransactionListOptions
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AccountId { get; set; }
    }
}
