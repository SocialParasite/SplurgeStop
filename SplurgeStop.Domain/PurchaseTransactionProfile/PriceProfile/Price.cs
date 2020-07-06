using System.Collections.Generic;

namespace SplurgeStop.Domain.PurchaseTransactionProfile.PriceProfile
{
    public class Price : Money
    {
        public Price(decimal amount,
                     Booking booking = Booking.Credit,
                     string currencyCode = "EUR",
                     string currencySymbol = "€",
                     CurrencySymbolPosition currencySymbolPosition = CurrencySymbolPosition.End)
           : base(amount, new Currency(currencyCode, currencySymbol, currencySymbolPosition))
        {
            Booking = booking;
        }

        public Booking Booking { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
        }
    }
}