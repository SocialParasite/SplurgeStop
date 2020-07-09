using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;
using Xunit;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/Country")]
    public sealed class CountryTests
    {
        [Fact]
        public void Country_created()
        {
            var countryId = new CountryId(SequentialGuid.NewSequentialGuid());
            var sut = Country.Create(countryId, "Dystopia");

            Assert.NotNull(sut);
            Assert.Contains("Dystopia", sut.Name);
        }

        public static List<object[]> InvalidCountryData = new List<object[]>
        {
            new object[] { null, "Dystopia" },
            new object[] { new CountryId(SequentialGuid.NewSequentialGuid()), null },
            new object[] { new CountryId(SequentialGuid.NewSequentialGuid()), "" },
            new object[] { null, null },
        };

        [Theory]
        [MemberData(nameof(InvalidCountryData))]
        public void Country_not_created(CountryId id, string name)
        {
            Action sut = () => Country.Create(id, name);

            Assert.Throws<ArgumentNullException>(sut.Invoke);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("Dystopia")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shou")]
        public void Valid_country_name(string name)
        {
            var sut = new Country();

            sut.UpdateCountryName(name);

            Assert.Equal(name, sut.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shoul")]
        public void Invalid_country_name(string name)
        {
            var sut = new Country();

            Action action = () => sut.UpdateCountryName(name);

            Assert.Throws<ArgumentException>(action.Invoke);
        }

        [Fact]
        public void Country_name_cannot_be_null()
        {
            var sut = new Country();

            Action action = () => sut.UpdateCountryName(null);

            Assert.Throws<ArgumentNullException>(action.Invoke);
        }
    }
}
