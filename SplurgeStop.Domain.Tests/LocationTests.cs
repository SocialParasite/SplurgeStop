using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;
using Xunit;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/Location")]
    public sealed class LocationTests
    {
        static readonly LocationId LocationId = new LocationId(SequentialGuid.NewSequentialGuid());
        static readonly CityId CityId = new CityId(SequentialGuid.NewSequentialGuid());
        static readonly CountryId CountryId = new CountryId(SequentialGuid.NewSequentialGuid());

        [Fact]
        public void Location_created()
        {
            var city = City.Create(CityId, "Rapture");
            var country = Country.Create(CountryId, "Dystopia");
            var sut = Location.Create(LocationId, city, country);

            Assert.NotNull(sut);
            Assert.Equal(CityId, sut.City.Id);
            Assert.Equal(CountryId, sut.Country.Id);
        }

        public static List<object[]> InvalidLocationData = new List<object[]>
        {
            new object[] { null, null, null },
            new object[] { LocationId, City.Create(CityId, "Rapture"), null },
            new object[] { LocationId, null, Country.Create(CountryId, "Dystopia") },
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
            var sut = Location.Create(LocationId, City.Create(CityId, "Rapture"),
                Country.Create(CountryId, "Dystopia"));
            var newCity = City.Create(new CityId(SequentialGuid.NewSequentialGuid()), "New Rapture");
            sut.UpdateCity(newCity);

            Assert.Equal("New Rapture", sut.City.Name);
        }

        [Fact]
        public void Invalid_city_update()
        {
            var sut = Location.Create(LocationId, City.Create(CityId, "Rapture"),
                Country.Create(CountryId, "Dystopia"));

            Assert.Throws<ArgumentNullException>(() => sut.UpdateCity(null));
        }

        [Fact]
        public void Valid_country_update()
        {
            var sut = Location.Create(LocationId, City.Create(CityId, "Rapture"),
                Country.Create(CountryId, "Dystopia"));
            var newCity = City.Create(new CityId(SequentialGuid.NewSequentialGuid()), "Dystopian Democratic Republic");
            sut.UpdateCity(newCity);

            Assert.Equal("Dystopian Democratic Republic", sut.City.Name);
        }

        [Fact]
        public void Invalid_country_update()
        {
            var sut = Location.Create(LocationId, City.Create(CityId, "Rapture"),
                Country.Create(CountryId, "Dystopia"));

            Assert.Throws<ArgumentNullException>(() => sut.UpdateCountry(null));
        }
    }
}
