using System;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.PriceProfile;

namespace SplurgeStop.Domain.PurchaseTransactionProfile.LineItemProfile
{
    public sealed class LineItemStripped
    {
        public Guid? Id { get; set; }
        public Guid ProductId { get; set; }
        public string Price { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public CurrencySymbolPosition CurrencySymbolPosition { get; set; }
        public Booking Booking { get; set; }
        public string Notes { get; set; }
    }
}
