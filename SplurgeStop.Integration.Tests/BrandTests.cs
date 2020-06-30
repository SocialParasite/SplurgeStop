using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using Xunit;
using static SplurgeStop.Integration.Tests.Helpers.BrandHelpers;

namespace SplurgeStop.Integration.Tests
{
    public sealed partial class DatabaseTests
    {
        [Fact]
        public async Task Brand_inserted_to_database()
        {
            Brand brand = await CreateValidBrand();

            var repository = new BrandRepository(fixture.context);
            var sut = await repository.LoadBrandAsync(brand.Id);

            Assert.True(await repository.ExistsAsync(sut.Id));
            Assert.True(sut.Name.Length > 0);
        }

        [Fact]
        public async Task Invalid_Brand()
        {
            var result = await CreateInvalidBrand();

            Assert.IsAssignableFrom<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Update_Brand_name()
        {
            Brand brand = await CreateValidBrand();

            var repository = new BrandRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(brand.Id));

            var sut = await repository.LoadBrandAsync(brand.Id);

            var brandId = sut.Id;

            Assert.NotNull(sut);
            Assert.Equal("Levi's", sut.Name);

            await UpdateBrandName(sut.Id, "Diesel");

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal("Diesel", sut.Name);
            Assert.Equal(brandId, sut.Id);
        }

        [Fact]
        public async Task Remove_Brand()
        {
            Brand brand = await CreateValidBrand();

            var sut = await CheckIfBrandExists(brand.Id);

            Assert.True(sut);

            await RemoveBrand(brand.Id);

            sut = await CheckIfBrandExists(brand.Id);

            Assert.False(sut);
        }
    }
}
