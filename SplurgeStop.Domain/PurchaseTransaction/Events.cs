using System;
using System.Collections.Generic;
using System.Text;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public static class Events
    {
        public class PurchaseTransactionCreated
        {
            public Guid Id { get; set; }
        }

        public class PurchaseTransactionDateChanged
        {
            public Guid Id { get; set; }
            public DateTime TransactionDate { get; set; }
        }
    }
}
