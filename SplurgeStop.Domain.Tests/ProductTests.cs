using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;
using Xunit;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/Product")]
    public sealed class ProductTests
    {
        [Fact]
        public void Product_created()
        {
            var productId = new ProductId(SequentialGuid.NewSequentialGuid());
            var brandId = Brand.Create(new BrandId(SequentialGuid.NewSequentialGuid()), "Duff");
            var sut = Product.Create(productId, "Duff", brandId);

            Assert.NotNull(sut);
            Assert.Contains("Duff", sut.Name);
        }

        public static List<object[]> InvalidProductData = new List<object[]>
        {
            new object[] { null, "Duff",  null },
            new object[] { new ProductId(SequentialGuid.NewSequentialGuid()), "", null },
            new object[] { null, null, null },
        };

        [Theory]
        [MemberData(nameof(InvalidProductData))]
        public void Product_not_created(ProductId id, string name, Brand brand)
        {
            Action sut = () => Product.Create(id, name, brand);

            Assert.Throws<ArgumentNullException>(sut.Invoke);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("Duff")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shou")]
        public void Valid_product_name(string name)
        {
            var sut = Product.Create(new ProductId(SequentialGuid.NewSequentialGuid()), "Duff",
                Brand.Create(new BrandId(SequentialGuid.NewSequentialGuid()), "Duff"));

            sut.UpdateProductName(name);

            Assert.Equal(name, sut.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shoul")]
        public void Invalid_product_name(string name)
        {
            var sut = Product.Create(new ProductId(SequentialGuid.NewSequentialGuid()), "Duff",
                Brand.Create(new BrandId(SequentialGuid.NewSequentialGuid()), "Duff"));

            Action action = () => sut.UpdateProductName(name);

            Assert.Throws<ArgumentException>(action.Invoke);
        }

        [Fact]
        public void Product_name_cannot_be_null()
        {
            var sut = Product.Create(new ProductId(SequentialGuid.NewSequentialGuid()), "Duff",
                Brand.Create(new BrandId(SequentialGuid.NewSequentialGuid()), "Duff"));

            Action action = () => sut.UpdateProductName(null);

            Assert.Throws<ArgumentNullException>(action.Invoke);
        }

        [Fact]
        public void Valid_brand_update()
        {

            var sut = Product.Create(new ProductId(SequentialGuid.NewSequentialGuid()), "Duff",
                Brand.Create(new BrandId(SequentialGuid.NewSequentialGuid()), "Duff"));

            var newBrand = Brand.Create(new BrandId(SequentialGuid.NewSequentialGuid()), "DuffZ");
            sut.UpdateBrand(newBrand);

            Assert.Equal("DuffZ", sut.Brand.Name);
        }

        [Fact]
        public void Invalid_brand_update()
        {
            var sut = Product.Create(new ProductId(SequentialGuid.NewSequentialGuid()), "Duff",
                Brand.Create(new BrandId(SequentialGuid.NewSequentialGuid()), "Duff"));

            Assert.Throws<ArgumentNullException>(() => sut.UpdateBrand(null));
        }

        [Fact]
        public void Valid_product_type_update()
        {

            var sut = Product.Create(new ProductId(SequentialGuid.NewSequentialGuid()), "Duff",
                Brand.Create(new BrandId(SequentialGuid.NewSequentialGuid()), "Duff"));

            var newProductType = ProductType.Create(new ProductTypeId(SequentialGuid.NewSequentialGuid()), "Duff IPA");
            sut.UpdateProductType(newProductType);

            Assert.Equal("Duff IPA", sut.ProductType.Name);
        }

        [Fact]
        public void Invalid_product_type_update()
        {
            var sut = Product.Create(new ProductId(SequentialGuid.NewSequentialGuid()), "Duff",
                Brand.Create(new BrandId(SequentialGuid.NewSequentialGuid()), "Duff"));

            Assert.Throws<ArgumentNullException>(() => sut.UpdateProductType(null));
        }

        [Fact]
        public void Valid_size_update()
        {

            var sut = Product.Create(new ProductId(SequentialGuid.NewSequentialGuid()), "Duff",
                Brand.Create(new BrandId(SequentialGuid.NewSequentialGuid()), "Duff"));

            var newSize = Size.Create(new SizeId(SequentialGuid.NewSequentialGuid()), "0.33");
            sut.UpdateSize(newSize);

            Assert.Equal("0.33", sut.Size.Amount);
        }

        [Fact]
        public void Invalid_size_update()
        {
            var sut = Product.Create(new ProductId(SequentialGuid.NewSequentialGuid()), "Duff",
                Brand.Create(new BrandId(SequentialGuid.NewSequentialGuid()), "Duff"));

            Assert.Throws<ArgumentNullException>(() => sut.UpdateSize(null));
        }
    }
}
