using System;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public class Currency
    {
        public string CurrencyCode { get; }
        public string CurrencySymbol { get; }
        public CurrencySymbolPosition PositionRelativeToPrice { get; }

        public Currency(string currencyCode, string currencySymbol, CurrencySymbolPosition position)
        {
            if (currencyCode == null)
                throw new ArgumentNullException(nameof(currencyCode), "Invalid Currency code!");

            if (currencyCode.Length != 3)
                throw new ArgumentException("Invalid Currency code! Currency code should be three characters long.", nameof(currencyCode));

            CurrencyCode = currencyCode;
            CurrencySymbol = currencySymbol;
            PositionRelativeToPrice = position;
        }
    }
}