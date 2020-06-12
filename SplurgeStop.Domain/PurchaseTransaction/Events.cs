using System;
using System.Collections.Generic;
using SplurgeStop.Domain.PurchaseTransaction.DTO;
using SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public static class Events
    {
        public class PurchaseTransactionCreated
        {
            public Guid Id { get; set; }
        }

        public class PurchaseTransactionFullCreated
        {
            public Guid? Id { get; set; }
            public DateTime TransactionDate { get; set; }
            public Store Store { get; set; }
            public ICollection<LineItemStripped> LineItems { get; set; }
        }

        public class PurchaseTransactionDateChanged
        {
            public Guid Id { get; set; }
            public DateTime TransactionDate { get; set; }
        }

        public class PurchaseTransactionStoreChanged
        {
            public Guid Id { get; set; }
            public Store Store { get; set; }
        }

        public class PurchaseTransactionLineItemChanged
        {
            public Guid Id { get; set; }
            public LineItem LineItem { get; set; }
        }

        public class LineItemChanged
        {
            public Guid Id { get; set; }
            public LineItem LineItem { get; set; }
        }

        public class PurchaseTransactionDeleted
        {
            public Guid Id { get; set; }
        }
    }
}
