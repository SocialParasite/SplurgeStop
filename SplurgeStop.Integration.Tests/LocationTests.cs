using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
using SplurgeStop.Integration.Tests.Helpers;
using Xunit;
using static SplurgeStop.Integration.Tests.Helpers.LocationHelpers;

namespace SplurgeStop.Integration.Tests
{
    public sealed partial class DatabaseTests
    {
        [Fact]
        public async Task Location_inserted_to_database()
        {
            Location location = await CreateValidLocation();

            var repository = new LocationRepository(fixture.context);
            var sut = await repository.LoadAsync(location.Id);

            Assert.True(await repository.ExistsAsync(sut.Id));
            Assert.NotNull(sut.City);
            Assert.NotNull(sut.Country);
        }

        [Theory]
        [InlineData("CityId")]
        [InlineData("CountryId")]
        public async Task Invalid_Location(string invalidProp)
        {
            var result = await CreateInvalidLocation(invalidProp);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Update_Location_city()
        {
            Location location = await CreateValidLocation();

            var repository = new LocationRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(location.Id));

            var sut = await repository.LoadAsync(location.Id);

            Assert.NotNull(sut);

            var newCity = await CityHelpers.CreateValidCity();
            await UpdateLocationCity(sut.Id, newCity);

            sut = await repository.LoadAsync(sut.Id);
            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(newCity.Id, sut.City.Id);
        }

        [Fact]
        public async Task Update_Location_country()
        {
            Location location = await CreateValidLocation();

            var repository = new LocationRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(location.Id));

            var sut = await repository.LoadAsync(location.Id);

            Assert.NotNull(sut);

            var newCountry = await CountryHelpers.CreateValidCountry();
            await UpdateLocationCountry(sut.Id, newCountry);

            sut = await repository.LoadAsync(sut.Id);
            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(newCountry.Id, sut.Country.Id);
        }

        [Fact]
        public async Task Remove_Location()
        {
            Location location = await CreateValidLocation();

            var sut = await CheckIfLocationExists(location.Id);

            Assert.True(sut);

            await RemoveLocation(location.Id);

            sut = await CheckIfLocationExists(location.Id);

            Assert.False(sut);
        }
    }
}
