using System;
using GuidHelpers;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.LineItemProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.PriceProfile;
using Xunit;
using Price = SplurgeStop.Domain.PurchaseTransactionProfile.PriceProfile.Price;

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
            var brand = Brand.Create(Guid.NewGuid(), "brand");
            var productType = ProductType.Create(Guid.NewGuid(), "product type");
            var size = Size.Create(Guid.NewGuid(), "your size");

            var product = Product.Create(Guid.NewGuid(), "product", brand, productType, size);

            var price = new Price(amount, booking, currencyCode, currencySymbol, currencySymbolPosition);
            var sut = LineItemBuilder.LineItem(price).WithProduct(product).Build();

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
            var brand = Brand.Create(Guid.NewGuid(), "brand");
            var productType = ProductType.Create(Guid.NewGuid(), "product type");
            var size = Size.Create(Guid.NewGuid(), "your size");

            var product = Product.Create(Guid.NewGuid(), "product", brand, productType, size);

            var price = new Price(1.00m);
            var sut = LineItemBuilder.LineItem(price)
                .WithNotes("No kun sai niin halvalla.")
                .WithProduct(product)
                .Build();

            Assert.Contains("halvalla", sut.Notes);
        }

        [Fact]
        public void Has_product()
        {
            var price = new Price(1.00m);

            var brand = Brand.Create(Guid.NewGuid(), "test");
            var productType = ProductType.Create(Guid.NewGuid(), "product type");
            var size = Size.Create(Guid.NewGuid(), "your size");

            var prodId = new ProductId(SequentialGuid.NewSequentialGuid());
            var product = Product.Create(prodId, "newProd", brand, productType, size);
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
