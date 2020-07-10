using System;
using System.Threading.Tasks;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Integration.Tests.Helpers;
using Xunit;
using static SplurgeStop.Integration.Tests.Helpers.ProductHelpers;

namespace SplurgeStop.Integration.Tests
{
    [Trait("Integration", "Database")]
    public sealed partial class DatabaseTests
    {
        // STORE
        [Fact]
        public async Task Product_inserted_to_database()
        {
            Product product = await CreateValidProduct();

            var repository = new ProductRepository(fixture.context);
            var sut = await repository.LoadAsync(product.Id);

            Assert.True(await repository.ExistsAsync(sut.Id));
            Assert.True(sut.Name.Length > 0);
        }

        [Fact]
        public async Task Invalid_Product()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await CreateInvalidProduct());
        }

        [Fact]
        public async Task Update_Product_name()
        {
            Product product = await CreateValidProduct();

            var repository = new ProductRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(product.Id));

            var sut = await repository.LoadFullProductTrackedAsync(product.Id);

            var productId = sut.Id;

            Assert.NotNull(sut);
            Assert.Equal("New product", sut.Name);

            await UpdateProductName(sut.Id, "Hot new product!", sut.Brand.Id);

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal("Hot new product!", sut.Name);
            Assert.Equal(productId, sut.Id);
        }

        [Fact]
        public async Task Update_product_brand()
        {
            var product = await CreateValidProduct();

            var repository = new ProductRepository(fixture.context);

            var sut = await repository.GetProductFullAsync(product.Id);
            Assert.True(await repository.ExistsAsync(product.Id));

            Assert.NotNull(sut);

            var newBrand = await BrandHelpers.CreateValidBrand();
            await UpdateProductBrand(sut.Id, newBrand);

            sut = await repository.LoadFullProductTrackedAsync(sut.Id);
            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(newBrand.Id, sut.Brand.Id);
        }

        [Fact]
        public async Task Update_product_type()
        {
            var product = await CreateValidProduct();

            var repository = new ProductRepository(fixture.context);

            var sut = await repository.GetProductFullAsync(product.Id);
            Assert.True(await repository.ExistsAsync(product.Id));

            Assert.NotNull(sut);

            var newProductType = await ProductTypeHelpers.CreateValidProductType();
            await UpdateProductType(sut.Id, newProductType);

            sut = await repository.LoadFullProductTrackedAsync(sut.Id);
            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(newProductType.Id, sut.ProductType.Id);
        }

        [Fact]
        public async Task Update_product_size()
        {
            var product = await CreateValidProduct();

            var repository = new ProductRepository(fixture.context);

            var sut = await repository.GetProductFullAsync(product.Id);
            Assert.True(await repository.ExistsAsync(product.Id));

            Assert.NotNull(sut);

            var newSize = await SizeHelpers.CreateValidSize();
            await UpdateProductSize(sut.Id, newSize);

            sut = await repository.LoadFullProductTrackedAsync(sut.Id);
            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(newSize.Id, sut.Size.Id);
        }

        [Fact]
        public async Task Remove_product()
        {
            Product product = await CreateValidProduct();

            var sut = await CheckIfProductExists(product.Id);

            Assert.True(sut);

            await RemoveProduct(product.Id);

            sut = await CheckIfProductExists(product.Id);

            Assert.False(sut);
        }
    }
}
