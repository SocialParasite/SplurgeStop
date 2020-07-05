using System;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.PurchaseTransaction.PriceProfile;

namespace SplurgeStop.Domain.PurchaseTransaction.DTO
{
    public sealed class LineItemStripped
    {
        public Guid? Id { get; set; }
        public Product Product { get; set; }
        public string Price { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public CurrencySymbolPosition CurrencySymbolPosition { get; set; }
        public Booking Booking { get; set; }
        public string Notes { get; set; }
    }
}
