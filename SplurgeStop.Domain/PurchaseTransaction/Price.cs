using System;
using System.Collections.Generic;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public class Price : ValueObject
    {
        public Price() { }

        private Price(decimal amount, string currencyCode)
        {
            if (amount < 0)
                throw new ArgumentException("Price cannot be negative", nameof(amount));

            Amount = amount;
            CurrencyCode = currencyCode;
        }
        
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
        }
    }

    public enum TEMP // OR allow negative amounts? //If this, should this be set on LineItem or here?
    {
        Debit,
        Credit
    }
}