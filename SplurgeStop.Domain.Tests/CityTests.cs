using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using Xunit;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/City")]
    public sealed class CityTests
    {
        [Fact]
        public void City_created()
        {
            var cityId = new CityId(SequentialGuid.NewSequentialGuid());
            var sut = City.Create(cityId, "Rapture");

            Assert.NotNull(sut);
            Assert.Contains("Rapture", sut.Name);
        }

        public static List<object[]> InvalidCityData = new List<object[]>
        {
            new object[] { null, "Rapture" },
            new object[] { new CityId(SequentialGuid.NewSequentialGuid()), null },
            new object[] { new CityId(SequentialGuid.NewSequentialGuid()), "" },
            new object[] { null, null },
        };

        [Theory]
        [MemberData(nameof(InvalidCityData))]
        public void City_not_created(CityId id, string name)
        {
            Action sut = () => City.Create(id, name);

            Assert.Throws<ArgumentNullException>(sut.Invoke);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("Rapture")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shou")]
        public void Valid_city_name(string name)
        {
            var sut = City.Create(Guid.NewGuid(), "city");

            sut.UpdateCityName(name);

            Assert.Equal(name, sut.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shoul")]
        public void Invalid_city_name(string name)
        {
            var sut = City.Create(Guid.NewGuid(), "city");

            Action action = () => sut.UpdateCityName(name);

            Assert.Throws<ArgumentException>(action.Invoke);
        }

        [Fact]
        public void City_name_cannot_be_null()
        {
            var sut = City.Create(Guid.NewGuid(), "city");

            Action action = () => sut.UpdateCityName(null);

            Assert.Throws<ArgumentNullException>(action.Invoke);
        }
    }
}
