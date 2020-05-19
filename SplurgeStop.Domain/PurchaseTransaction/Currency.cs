using System;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public class Currency
    {
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public CurrencySymbolPosition PositionRelativeToPrice { get; set; }

        public Currency(string currencyCode, string currencySymbol, CurrencySymbolPosition position)
        {
            if (currencyCode == null)
                throw new ArgumentNullException("Invalid Currency code!", nameof(currencyCode));

            if (currencyCode.Length != 3)
                throw new ArgumentException("Invalid Currency code! Currency code should be three characters long.", nameof(currencyCode));

            CurrencyCode = currencyCode;
            CurrencySymbol = currencySymbol;
            PositionRelativeToPrice = position;
        }
    }

    public enum CurrencySymbolPosition
    {
        front,
        end
    }
}