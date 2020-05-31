using System;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public static class Commands
    {
        public class Create
        {
            public Guid? Id { get; set; }
        }

        public class SetPurchaseTransactionDate
        {
            public Guid Id { get; set; }
            public DateTime TransactionDate { get; set; }
        }

        public class SetPurchaseTransactionStore
        {
            public Guid Id { get; set; }
            public Guid StoreId { get; set; }
        }

        public class SetPurchaseTransactionLineItem
        {
            public Guid Id { get; set; }
            public Guid LineItemId { get; set; }
            public decimal Price { get; set; }
            public string Currency { get; set; }
            public string CurrencySymbol { get; set; }
            public CurrencySymbolPosition CurrencySymbolPosition { get; set; }
            public Booking Booking { get; set; }
            public string Notes { get; set; }
        }

        public class UpdateLineItem
        {
            public Guid Id { get; set; }
            public LineItem LineItem { get; set; }
        }
    }
}
