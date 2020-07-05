using System;
using SplurgeStop.Domain.PurchaseTransaction.PriceProfile;
using Xunit;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/Money")]
    public class MoneyTests
    {
        [Fact]
        public void Equal_money()
        {
            var firstAmount = new Money(5, "EUR", "€", CurrencySymbolPosition.End);
            var secondAmount = new Money(5, "EUR", "€", CurrencySymbolPosition.Front);

            Assert.Equal(firstAmount, secondAmount);
        }

        [Fact]
        public void Unequal_money()
        {
            var firstAmount = new Money(5, "EUR", "€", CurrencySymbolPosition.End);
            var secondAmount = new Money(5, "USD", "$", CurrencySymbolPosition.Front);

            Assert.NotEqual(firstAmount, secondAmount);
        }

        [Fact]
        public void Sum_of_money_equals_same()
        {
            var coin1 = new Money(1, "EUR", "€", CurrencySymbolPosition.End);
            var coin2 = new Money(2, "EUR", "€", CurrencySymbolPosition.End);
            var coin3 = new Money(2, "EUR", "€", CurrencySymbolPosition.End);

            var banknote = new Money(5, "EUR", "€", CurrencySymbolPosition.End);

            Assert.Equal(banknote, coin1 + coin2 + coin3);
        }

        [Fact]
        public void Subtract_money_equals_same()
        {
            var money1 = new Money(10, "EUR", "€", CurrencySymbolPosition.End);
            var money2 = new Money(5, "EUR", "€", CurrencySymbolPosition.End);
            var money3 = new Money(5, "EUR", "€", CurrencySymbolPosition.End);

            Assert.Equal(money3, money1 - money2);
            Assert.Equal(money3, money2 - money1);
        }

        [Fact]
        public void Currency_symbol_suffixed()
        {
            var money = new Money(10, "EUR", "€", CurrencySymbolPosition.End);

            var sut = money.ToString();

            Assert.EndsWith("€", sut);
        }

        [Fact]
        public void Currency_symbol_prefixed()
        {
            var money = new Money(10, "USD", "$", CurrencySymbolPosition.Front);

            var sut = money.ToString();

            Assert.StartsWith("$", sut);
        }

        [Fact]
        public void Throws_on_adding_different_currencies()
        {
            var firstAmount = new Money(5, "EUR", "€", CurrencySymbolPosition.End);
            var secondAmount = new Money(5, "USD", "$", CurrencySymbolPosition.Front);

            Assert.Throws<ArgumentException>(() => firstAmount + secondAmount);
        }

        [Fact]
        public void Throws_on_substracting_different_currencies()
        {
            var firstAmount = new Money(5, "EUR", "€", CurrencySymbolPosition.End);
            var secondAmount = new Money(5, "USD", "$", CurrencySymbolPosition.Front);

            Assert.Throws<ArgumentException>(() => firstAmount - secondAmount);
        }
    }
}
