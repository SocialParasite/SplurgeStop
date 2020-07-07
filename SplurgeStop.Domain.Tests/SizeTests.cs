using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using Xunit;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/Size")]
    public sealed class SizeTests
    {
        [Fact]
        public void Size_created()
        {
            var sizeId = new SizeId(SequentialGuid.NewSequentialGuid());
            var sut = Size.Create(sizeId, "Perfect fit");

            Assert.NotNull(sut);
            Assert.Contains("Perfect", sut.Amount);
        }

        public static List<object[]> InvalidSizeData = new List<object[]>
        {
            new object[] { null, "Perfect fit" },
            new object[] { new SizeId(SequentialGuid.NewSequentialGuid()), null },
            new object[] { null, null },
        };

        [Theory]
        [MemberData(nameof(InvalidSizeData))]
        public void Size_not_created(SizeId id, string amount)
        {
            Action sut = () => Size.Create(id, amount);

            Assert.Throws<ArgumentNullException>(sut.Invoke);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("New size")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shou")]
        public void Valid_size_amount(string amount)
        {
            var sut = new Size();

            sut.UpdateSizeAmount(amount);

            Assert.Equal(amount, sut.Amount);
        }

        [Theory]
        [InlineData("")]
        public void Invalid_size_amount(string amount)
        {
            var sut = new Size();

            Action action = () => sut.UpdateSizeAmount(amount);

            Assert.Throws<ArgumentException>(action.Invoke);
        }

        [Fact]
        public void Size_amount_cannot_be_null()
        {
            var sut = new Size();

            Action action = () => sut.UpdateSizeAmount(null);

            Assert.Throws<ArgumentNullException>(action.Invoke);
        }
    }
}
