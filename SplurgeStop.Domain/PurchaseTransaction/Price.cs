using System;
using System.Collections.Generic;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    //[Obsolete]
    //public class Price : ValueObject
    //{
    //    public Price() { }

    //    private Price(decimal amount, string currencyCode)
    //    {
    //        if (amount < 0)
    //            throw new ArgumentException("Price cannot be negative", nameof(amount));

    //        Amount = amount;
    //        CurrencyCode = currencyCode;
    //    }
        
    //    public decimal Amount { get; set; }
    //    public string CurrencyCode { get; set; }

    //    protected override IEnumerable<object> GetEqualityComponents()
    //    {
    //        yield return Amount;
    //    }
    //}

    public enum Booking
    {
        Credit,
        Debit
    }


    public class Price : Money
    {
        public Price(decimal amount,
                     Booking booking = Booking.Credit,
                     string currencyCode = "EUR",
                     string currencySymbol = "€",
                     CurrencySymbolPosition currencySymbolPosition = CurrencySymbolPosition.end)
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