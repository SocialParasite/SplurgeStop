using System;
using GuidHelpers;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.PurchaseTransaction.PriceProfile;
using Xunit;
using Price = SplurgeStop.Domain.PurchaseTransaction.PriceProfile.Price;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/LineItem")]
    public sealed class LineItemTests
    {
        [Fact]
        public void Valid_lineItem()
        {
            var price = new Price(1.00m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.End);
            var sut = LineItemBuilder.LineItem(price).Build();

            Assert.IsType<LineItem>(sut);
            Assert.Equal(1.00m, sut.Price.Amount);
        }

        [Fact]
        public void Has_notes()
        {
            var price = new Price(1.00m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.End);
            var sut = LineItemBuilder.LineItem(price).WithNotes("No kun sai niin halvalla.").Build();

            Assert.Contains("halvalla", sut.Notes);
        }

        [Fact]
        public void Has_product()
        {
            var price = new Price(1.00m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.End);

            var brand = new Brand();
            var prodId = new ProductId(SequentialGuid.NewSequentialGuid());
            var product = Product.Create(prodId, "newProd", brand);
            var sut = LineItemBuilder.LineItem(price).WithProduct(product).Build();

            Assert.Contains("newProd", sut.Product.Name);
        }

        [Fact]
        public void Invalid_lineItem()
        {
            Assert.Throws<ArgumentNullException>(()
                => LineItemBuilder.LineItem(null).Build());
        }
    }
}
