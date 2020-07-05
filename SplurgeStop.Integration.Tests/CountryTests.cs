using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.CountryProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;
using Xunit;
using static SplurgeStop.Integration.Tests.Helpers.CountryHelpers;

namespace SplurgeStop.Integration.Tests
{
    public sealed partial class DatabaseTests
    {
        [Fact]
        public async Task Country_inserted_to_database()
        {
            Country country = await CreateValidCountry();

            var repository = new CountryRepository(fixture.context);
            var sut = await repository.LoadCountryAsync(country.Id);

            Assert.True(await repository.ExistsAsync(sut.Id));
            Assert.True(sut.Name.Length > 0);
        }

        [Fact]
        public async Task Invalid_Country()
        {
            var result = await CreateInvalidCountry();

            Assert.IsAssignableFrom<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Update_Country_name()
        {
            Country country = await CreateValidCountry();

            var repository = new CountryRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(country.Id));

            var sut = await repository.LoadCountryAsync(country.Id);

            var countryId = sut.Id;

            Assert.NotNull(sut);
            Assert.Equal("Rapture", sut.Name);

            await UpdateCountryName(sut.Id, "Columbia");

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal("Columbia", sut.Name);
            Assert.Equal(countryId, sut.Id);
        }

        [Fact]
        public async Task Remove_Country()
        {
            Country country = await CreateValidCountry();

            var sut = await CheckIfCountryExists(country.Id);

            Assert.True(sut);

            await RemoveCountry(country.Id);

            sut = await CheckIfCountryExists(country.Id);

            Assert.False(sut);
        }
    }
}
