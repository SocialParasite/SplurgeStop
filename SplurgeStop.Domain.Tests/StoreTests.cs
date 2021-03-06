﻿using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;
using Xunit;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/Store")]
    public sealed class StoreTests
    {
        [Fact]
        public void Store_created()
        {
            var storeId = new StoreId(SequentialGuid.NewSequentialGuid());
            var sut = Store.Create(storeId, "Kwik-E-Mart");

            Assert.NotNull(sut);
            Assert.Contains("Kwik-E-Mart", sut.Name);
        }

        public static List<object[]> InvalidStoreData = new List<object[]>
        {
            new object[] { null, "Kwik-E-Mart" },
            new object[] { new StoreId(SequentialGuid.NewSequentialGuid()), null },
            new object[] { new StoreId(SequentialGuid.NewSequentialGuid()), "" },
            new object[] { null, null },
        };

        [Theory]
        [MemberData(nameof(InvalidStoreData))]
        public void Store_not_created(StoreId id, string name)
        {
            Action sut = () => Store.Create(id, name);

            Assert.Throws<ArgumentNullException>(sut.Invoke);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("City-Wok")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shou")]
        public void Valid_store_name(string name)
        {
            var sut = Store.Create(new StoreId(SequentialGuid.NewSequentialGuid()), "Kwik-E-Mart");

            sut.UpdateStoreName(name);

            Assert.Equal(name, sut.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Bacon ipsum dolor amet burgdoggen beef turkey venison landjaeger frankfurter bresaola, andouille tail beef ribs. Burgdoggen shoul")]
        public void Invalid_store_name(string name)
        {
            var sut = Store.Create(new StoreId(SequentialGuid.NewSequentialGuid()), "Kwik-E-Mart");

            Action action = () => sut.UpdateStoreName(name);

            Assert.Throws<ArgumentException>(action.Invoke);
        }

        [Fact]
        public void Store_name_cannot_be_null()
        {
            var sut = Store.Create(new StoreId(SequentialGuid.NewSequentialGuid()), "Kwik-E-Mart");

            Action action = () => sut.UpdateStoreName(null);

            Assert.Throws<ArgumentNullException>(action.Invoke);
        }

        [Fact]
        public void Valid_location_update()
        {
            var newLocation = Location.Create(new LocationId(SequentialGuid.NewSequentialGuid()),
                                City.Create(new CityId(SequentialGuid.NewSequentialGuid()), "Rapture"),
                                Country.Create(new CountryId(SequentialGuid.NewSequentialGuid()), "Dystopia"));

            var sut = Store.Create(new StoreId(SequentialGuid.NewSequentialGuid()), "Kwik-E-Mart");
            sut.UpdateLocation(newLocation);

            Assert.Equal(newLocation.Id, sut.Location.Id);
        }

        [Fact]
        public void Invalid_location_update()
        {
            var sut = Store.Create(new StoreId(SequentialGuid.NewSequentialGuid()), "Kwik-E-Mart");

            Assert.Throws<ArgumentNullException>(() => sut.UpdateLocation(null));
        }
    }
}
