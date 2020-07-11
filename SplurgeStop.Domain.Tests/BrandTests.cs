using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using Xunit;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/Brand")]
    public sealed class BrandTests
    {
        [Fact]
        public void Brand_created()
        {
            var brandId = new BrandId(SequentialGuid.NewSequentialGuid());
            var sut = Brand.Create(brandId, "My brand");

            Assert.NotNull(sut);
            Assert.Contains("My brand", sut.Name);
        }

        public static List<object[]> InvalidBrandData = new List<object[]>
        {
            new object[] { null, "My brand" },
            new object[] { new BrandId(SequentialGuid.NewSequentialGuid()), null },
            new object[] { new BrandId(SequentialGuid.NewSequentialGuid()), "" },
            new object[] { null, null },
        };

        [Theory]
        [MemberData(nameof(InvalidBrandData))]
        public void Brand_not_created(BrandId id, string name)
        {
            Action sut = () => Brand.Create(id, name);

            Assert.Throws<ArgumentNullException>(sut.Invoke);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("New brand")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shou")]
        public void Valid_brand_name(string name)
        {
            var sut = Brand.Create(Guid.NewGuid(), "test");

            sut.UpdateBrandName(name);

            Assert.Equal(name, sut.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shoul")]
        public void Invalid_brand_name(string name)
        {
            var sut = Brand.Create(Guid.NewGuid(), "test");

            Action action = () => sut.UpdateBrandName(name);

            Assert.Throws<ArgumentException>(action.Invoke);
        }

        [Fact]
        public void Brand_name_cannot_be_null()
        {
            var sut = Brand.Create(Guid.NewGuid(), "test");

            Action action = () => sut.UpdateBrandName(null);

            Assert.Throws<ArgumentNullException>(action.Invoke);
        }
    }
}
