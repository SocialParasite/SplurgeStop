using System;
using System.Collections.Generic;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.LineItemProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.PriceProfile;

namespace SplurgeStop.Domain.PurchaseTransactionProfile
{
    public static class Commands
    {
        public class Create
        {
            public Guid? Id { get; set; }
        }

        public class CreateFull
        {
            public Guid? Id { get; set; }
            public DateTime TransactionDate { get; set; }
            public Guid StoreId { get; set; }
            public string Notes { get; set; }
            public ICollection<LineItemStripped> LineItems { get; set; }
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
            public Guid? LineItemId { get; set; }
            public decimal Price { get; set; }
            public string Currency { get; set; }
            public string CurrencySymbol { get; set; }
            public CurrencySymbolPosition CurrencySymbolPosition { get; set; }
            public Booking Booking { get; set; }
            public string Notes { get; set; }
            public Product Product { get; set; }
        }

        public class DeletePurchaseTransaction
        {
            public Guid Id { get; set; }
        }

        public class UpdateLineItem
        {
            public Guid Id { get; set; }
            public LineItem LineItem { get; set; }
        }
    }
}
