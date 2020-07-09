using System;
using System.Threading.Tasks;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.ProductProfile.TypeProfile;
using Xunit;
using static SplurgeStop.Integration.Tests.Helpers.ProductTypeHelpers;

namespace SplurgeStop.Integration.Tests
{
    public sealed partial class DatabaseTests
    {
        [Fact]
        public async Task ProductType_inserted_to_database()
        {
            ProductType productType = await CreateValidProductType();

            var repository = new ProductTypeRepository(fixture.context);
            var sut = await repository.LoadAsync(productType.Id);

            Assert.True(await repository.ExistsAsync(sut.Id));
            Assert.True(sut.Name.Length > 0);
        }

        [Fact]
        public async Task Invalid_ProductType()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await CreateInvalidProductType());
        }

        [Fact]
        public async Task Update_ProductType_name()
        {
            ProductType productType = await CreateValidProductType();

            var repository = new ProductTypeRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(productType.Id));

            var sut = await repository.LoadAsync(productType.Id);

            var productTypeId = sut.Id;

            Assert.NotNull(sut);
            Assert.Equal("trousers", sut.Name);

            await UpdateProductTypeName(sut.Id, "jeans");

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal("jeans", sut.Name);
            Assert.Equal(productTypeId, sut.Id);
        }

        [Fact]
        public async Task Remove_ProductType()
        {
            ProductType productType = await CreateValidProductType();

            var sut = await CheckIfProductTypeExists(productType.Id);

            Assert.True(sut);

            await RemoveProductType(productType.Id);

            sut = await CheckIfProductTypeExists(productType.Id);

            Assert.False(sut);
        }
    }
}
