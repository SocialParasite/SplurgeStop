using System;
using GuidHelpers;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.PurchaseTransaction.LineItem;
using SplurgeStop.Domain.PurchaseTransaction.PriceProfile;
using Xunit;
using Price = SplurgeStop.Domain.PurchaseTransaction.PriceProfile.Price;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/LineItem")]
    public sealed class LineItemTests
    {
        [Theory]
        [InlineData(1.00, Booking.Credit, "EUR", "€", CurrencySymbolPosition.End)]
        [InlineData(1.00, Booking.Debit, "EUR", "€", CurrencySymbolPosition.End)]
        [InlineData(1.00, Booking.Credit, "CAD", "$", CurrencySymbolPosition.Front)]
        public void Valid_lineItem(decimal amount,
                                   Booking booking,
                                   string currencyCode,
                                   string currencySymbol,
                                   CurrencySymbolPosition currencySymbolPosition)
        {
            var price = new Price(amount, booking, currencyCode, currencySymbol, currencySymbolPosition);
            var sut = LineItemBuilder.LineItem(price).WithProduct(new Product()).Build();

            Assert.IsType<LineItem>(sut);
            Assert.Equal(amount, sut.Price.Amount);
            Assert.Equal(booking, sut.Price.Booking);
            Assert.Equal(currencyCode, sut.Price.Currency.CurrencyCode);
            Assert.Equal(currencySymbol, sut.Price.Currency.CurrencySymbol);
            Assert.Equal(currencySymbolPosition, sut.Price.Currency.PositionRelativeToPrice);
        }

        [Fact]
        public void Has_notes()
        {
            var price = new Price(1.00m);
            var sut = LineItemBuilder.LineItem(price)
                .WithNotes("No kun sai niin halvalla.")
                .WithProduct(new Product())
                .Build();

            Assert.Contains("halvalla", sut.Notes);
        }

        [Fact]
        public void Has_product()
        {
            var price = new Price(1.00m);

            var brand = new Brand();
            var prodId = new ProductId(SequentialGuid.NewSequentialGuid());
            var product = Product.Create(prodId, "newProd", brand);
            var sut = LineItemBuilder.LineItem(price).WithProduct(product).Build();

            Assert.Contains("newProd", sut.Product.Name);
        }

        [Fact]
        public void Invalid_lineItem_no_price()
        {
            Assert.Throws<ArgumentNullException>(()
                => LineItemBuilder.LineItem(null).Build());
        }

        [Fact]
        public void Invalid_lineItem_no_product()
        {
            Assert.Throws<ArgumentNullException>(()
                => LineItemBuilder.LineItem(new Price(1.00m)).Build());
        }
    }
}
