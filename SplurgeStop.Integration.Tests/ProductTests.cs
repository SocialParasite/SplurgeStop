using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Integration.Tests.Helpers;
using Xunit;
using static SplurgeStop.Integration.Tests.Helpers.ProductHelpers;
using product = SplurgeStop.Domain.ProductProfile;

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
            var sut = await repository.LoadProductAsync(product.Id);

            Assert.True(await repository.ExistsAsync(sut.Id));
            Assert.True(sut.Name.Length > 0);
        }

        [Fact]
        public async Task Invalid_Product()
        {
            var result = await CreateInvalidProduct();

            Assert.IsAssignableFrom<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Update_Product_name()
        {
            Product product = await CreateValidProduct();

            var repository = new ProductRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(product.Id));

            var sut = await repository.LoadFullProductAsync(product.Id);

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

            sut = await repository.LoadFullProductAsync(sut.Id);
            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(newBrand.Id, sut.Brand.Id);
        }

        [Fact]
        public async Task Update_product_type()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task Update_product_size()
        {
            Assert.True(false);
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
