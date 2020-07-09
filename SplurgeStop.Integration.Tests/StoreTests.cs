using System;
using System.Threading.Tasks;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.Integration.Tests.Helpers;
using Xunit;
using static SplurgeStop.Integration.Tests.Helpers.StoreHelpers;
using store = SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Integration.Tests
{
    [Trait("Integration", "Database")]
    public sealed partial class DatabaseTests
    {
        // STORE
        [Fact]
        public async Task Store_inserted_to_database()
        {
            Store store = await CreateValidStore();

            var repository = new StoreRepository(fixture.context);
            var sut = await repository.LoadAsync(store.Id);

            Assert.True(await repository.ExistsAsync(sut.Id));
            Assert.True(sut.Name.Length > 0);
        }

        [Fact]
        public async Task Invalid_Store()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await CreateInvalidStore());
        }

        [Fact]
        public async Task Update_Store_name()
        {
            Store store = await CreateValidStore();

            var repository = new StoreRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(store.Id));

            var sut = await repository.GetTrackedAsync(store.Id);

            var storeId = sut.Id;

            Assert.NotNull(sut);
            Assert.Equal("New store", sut.Name);

            await UpdateStoreName(sut.Id, "Mega Market", sut.Location.Id);

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal("Mega Market", sut.Name);
            Assert.Equal(storeId, sut.Id);
        }

        [Fact]
        public async Task Update_store_location()
        {
            Store store = await CreateValidStore();

            var repository = new StoreRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(store.Id));

            var sut = await repository.LoadAsync(store.Id);

            Assert.NotNull(sut);

            var newLocation = await LocationHelpers.CreateValidLocation();
            await UpdateStoreLocation(sut.Id, newLocation);

            sut = await repository.GetTrackedAsync(sut.Id);
            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(newLocation.Id, sut.Location.Id);
        }

        [Fact]
        public async Task Remove_Store()
        {
            Store store = await CreateValidStore();

            var sut = await CheckIfStoreExists(store.Id);

            Assert.True(sut);

            await RemoveStore(store.Id);

            sut = await CheckIfStoreExists(store.Id);

            Assert.False(sut);
        }
    }
}
