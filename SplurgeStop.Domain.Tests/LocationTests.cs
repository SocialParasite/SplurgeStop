using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;
using Xunit;
using Xunit.Sdk;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/Location")]
    public sealed class LocationTests
    {
        static readonly LocationId _locationId = new LocationId(SequentialGuid.NewSequentialGuid());
        static readonly CityId _cityId = new CityId(SequentialGuid.NewSequentialGuid());
        static readonly CountryId _countryId = new CountryId(SequentialGuid.NewSequentialGuid());

        [Fact]
        public void Location_created()
        {
            var city = City.Create(_cityId, "Rapture");
            var country = Country.Create(_countryId, "Dystopia");
            var sut = Location.Create(_locationId, city, country);

            Assert.NotNull(sut);
            Assert.Equal(_cityId, sut.City.Id);
            Assert.Equal(_countryId, sut.Country.Id);
        }

        public static List<object[]> InvalidLocationData = new List<object[]>
        {
            new object[] { null, null, null },
            new object[] { _locationId, City.Create(_cityId, "Rapture"), null },
            new object[] { _locationId, null, Country.Create(_countryId, "Dystopia") },
        };

        [Theory]
        [MemberData(nameof(InvalidLocationData))]
        public void Location_not_created(LocationId locationId, City city, Country country)
        {
            Action sut = () => Location.Create(locationId, city, country);

            Assert.Throws<ArgumentNullException>(sut.Invoke);
        }

        [Fact]
        public void Valid_city_update()
        {
            var sut = Location.Create(_locationId, City.Create(_cityId, "Rapture"),
                Country.Create(_countryId, "Dystopia"));
            var newCity = City.Create(new CityId(SequentialGuid.NewSequentialGuid()), "New Rapture");
            sut.UpdateCity(newCity);

            Assert.Equal("New Rapture", sut.City.Name);
        }

        [Fact]
        public void Invalid_city_update()
        {
            var sut = Location.Create(_locationId, City.Create(_cityId, "Rapture"),
                Country.Create(_countryId, "Dystopia"));

            Assert.Throws<ArgumentNullException>(() => sut.UpdateCity(null));
        }

        [Fact]
        public void Valid_country_update()
        {
            var sut = Location.Create(_locationId, City.Create(_cityId, "Rapture"),
                Country.Create(_countryId, "Dystopia"));
            var newCity = City.Create(new CityId(SequentialGuid.NewSequentialGuid()), "Dystopian Democratic Republic");
            sut.UpdateCity(newCity);

            Assert.Equal("Dystopian Democratic Republic", sut.City.Name);
        }

        [Fact]
        public void Invalid_country_update()
        {
            var sut = Location.Create(_locationId, City.Create(_cityId, "Rapture"),
                Country.Create(_countryId, "Dystopia"));

            Assert.Throws<ArgumentNullException>(() => sut.UpdateCountry(null));
        }
    }
}
