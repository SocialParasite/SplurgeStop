using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;
using Xunit;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/ProductTypes")]
    public sealed class ProductTypeTests
    {
        [Fact]
        public void Brand_created()
        {
            var productTypeId = new ProductTypeId(SequentialGuid.NewSequentialGuid());
            var sut = ProductType.Create(productTypeId, "New ProductType");

            Assert.NotNull(sut);
            Assert.Contains("New ProductType", sut.Name);
        }

        public static List<object[]> InvalidProductTypeData = new List<object[]>
        {
            new object[] { null, "New ProductType" },
            new object[] { new ProductTypeId(SequentialGuid.NewSequentialGuid()), null },
            new object[] { new ProductTypeId(SequentialGuid.NewSequentialGuid()), "" },
            new object[] { null, null },
        };

        [Theory]
        [MemberData(nameof(InvalidProductTypeData))]
        public void ProductType_not_created(ProductTypeId id, string name)
        {
            Action sut = () => ProductType.Create(id, name);

            Assert.Throws<ArgumentNullException>(sut.Invoke);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("New ProductType")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shou")]
        public void Valid_product_type_name(string name)
        {
            var sut = ProductType.Create(Guid.NewGuid(), "type");

            sut.UpdateProductTypeName(name);

            Assert.Equal(name, sut.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shoul")]
        public void Invalid_product_type_name(string name)
        {
            var sut = ProductType.Create(Guid.NewGuid(), "type");

            Action action = () => sut.UpdateProductTypeName(name);

            Assert.Throws<ArgumentException>(action.Invoke);
        }

        [Fact]
        public void Product_type_name_cannot_be_null()
        {
            var sut = ProductType.Create(Guid.NewGuid(), "type");

            Action action = () => sut.UpdateProductTypeName(null);

            Assert.Throws<ArgumentNullException>(action.Invoke);
        }
    }
}
