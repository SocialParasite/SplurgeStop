using System;
using SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public static class Commands
    {
        public class Create
        {
            public PurchaseTransaction Transaction { get; set; }
        }

        public class SetPurchaseTransactionDate
        {
            public Guid Id { get; set; }
            public DateTime TransactionDate { get; set; }
        }

        public class SetPurchaseTransactionStore
        {
            public Guid Id { get; set; }
            public Store Store { get; set; }
        }
    }
}
