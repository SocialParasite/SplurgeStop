using System;
using System.Collections.Generic;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public class Money : ValueObject
    {
        protected Money() { }

        protected Money(decimal amount, Currency currency)
        {
            if (amount < 0)
                throw new ArgumentException("Price cannot be negative", nameof(amount));

            if (currency is null)
                throw new ArgumentNullException(nameof(currency), "Currency must be specified");

            Amount = amount;
            Currency = currency;
        }

        protected Money(decimal amount, string currencyCode, string currencySymbol, CurrencySymbolPosition currencySymbolPosition)
            : this (amount, new Currency(currencyCode, currencySymbol, currencySymbolPosition))
            {
        }


        public decimal Amount { get; internal set; }
        public Currency Currency { get; internal set; }

        public Money Add(Money summand)
        {
            if (Currency.CurrencyCode != summand.Currency.CurrencyCode)
                throw new ArgumentException("Cannot sum amounts with different currencies");

            return new Money(Amount + summand.Amount, Currency.CurrencyCode, Currency.CurrencySymbol, Currency.PositionRelativeToPrice);
        }

        public Money Subtract(Money subtrahend)
        {
            if (Currency != subtrahend.Currency)
                throw new ArgumentException("Cannot subtract amounts with different currencies");

            return new Money(Amount - subtrahend.Amount, Currency.CurrencyCode, Currency.CurrencySymbol, Currency.PositionRelativeToPrice);
        }

        public static Money operator +(Money summand1, Money summand2) 
            => summand1.Add(summand2);

        public static Money operator -(Money minuend, Money subtrahend) 
            => minuend.Subtract(subtrahend);

        public override string ToString()
        {
            return Currency.PositionRelativeToPrice == CurrencySymbolPosition.front
                ? $"{Currency.CurrencySymbol} {Amount}"
                : $"{Amount} {Currency.CurrencySymbol}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
        }
    }
}
