using System;
using SplurgeStop.Domain.PurchaseTransaction;
using Xunit;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/LineItem")]
    public sealed class LineItemTests
    {
        [Fact]
        public void Valid_lineItem()
        {
            var price = new Price(1.00m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end);
            var sut = LineItemBuilder.LineItem(price).Build();

            Assert.IsType<LineItem>(sut);
            Assert.Equal(1.00m, sut.Price.Amount);
        }

        [Fact]
        public void Has_notes()
        {
            var price = new Price(1.00m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end);
            var sut = LineItemBuilder.LineItem(price).WithNotes("No kun sai niin halvalla.").Build();

            Assert.Contains("halvalla", sut.Notes);
        }

        [Fact]
        public void Invalid_lineItem()
        {
            Assert.Throws<ArgumentNullException>(()
                => LineItemBuilder.LineItem(null).Build());
        }
    }
}
