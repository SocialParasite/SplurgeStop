using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using Xunit;
using static SplurgeStop.Integration.Tests.Helpers.CityHelpers;

namespace SplurgeStop.Integration.Tests
{
    public sealed partial class DatabaseTests
    {
        [Fact]
        public async Task City_inserted_to_database()
        {
            City city = await CreateValidCity();

            var repository = new CityRepository(fixture.context);
            var sut = await repository.LoadAsync(city.Id);

            Assert.True(await repository.ExistsAsync(sut.Id));
            Assert.True(sut.Name.Length > 0);
        }

        [Fact]
        public async Task Invalid_City()
        {
            var result = await CreateInvalidCity();

            Assert.IsAssignableFrom<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Update_City_name()
        {
            City city = await CreateValidCity();

            var repository = new CityRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(city.Id));

            var sut = await repository.LoadAsync(city.Id);

            var cityId = sut.Id;

            Assert.NotNull(sut);
            Assert.Equal("New Mansester", sut.Name);

            await UpdateCityName(sut.Id, "Manse");

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal("Manse", sut.Name);
            Assert.Equal(cityId, sut.Id);
        }

        [Fact]
        public async Task Remove_City()
        {
            City city = await CreateValidCity();

            var sut = await CheckIfCityExists(city.Id);

            Assert.True(sut);

            await RemoveCity(city.Id);

            sut = await CheckIfCityExists(city.Id);

            Assert.False(sut);
        }
    }
}
