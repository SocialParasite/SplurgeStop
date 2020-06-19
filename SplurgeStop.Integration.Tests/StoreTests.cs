using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.StoreProfile;
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
            var sut = await repository.LoadStoreAsync(store.Id);

            Assert.True(await repository.ExistsAsync(sut.Id));
            Assert.True(sut.Name.Length > 0);
        }

        [Fact]
        public async Task Invalid_Store()
        {
            var result = await CreateInvalidStore();

            Assert.IsAssignableFrom<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Update_Store_name()
        {
            Store store = await CreateValidStore();

            var repository = new StoreRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(store.Id));

            var sut = await repository.LoadFullStoreAsync(store.Id);

            var storeId = sut.Id;

            Assert.NotNull(sut);
            Assert.Equal("Test market", sut.Name);

            await UpdateStoreName(sut.Id, "Mega Market");

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal("Mega Market", sut.Name);
            Assert.Equal(storeId, sut.Id);
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
