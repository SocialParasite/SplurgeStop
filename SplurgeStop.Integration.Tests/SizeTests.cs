using System;
using System.Threading.Tasks;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using Xunit;
using static SplurgeStop.Integration.Tests.Helpers.SizeHelpers;

namespace SplurgeStop.Integration.Tests
{
    public sealed partial class DatabaseTests
    {
        [Fact]
        public async Task Size_inserted_to_database()
        {
            Size size = await CreateValidSize();

            var repository = new SizeRepository(fixture.context);
            var sut = await repository.LoadAsync(size.Id);

            Assert.True(await repository.ExistsAsync(sut.Id));
            Assert.NotEmpty(sut.Amount);
        }

        [Fact]
        public async Task Invalid_size()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await CreateInvalidSize());
        }

        [Fact]
        public async Task Update_Size_name()
        {
            Size size = await CreateValidSize();

            var repository = new SizeRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(size.Id));

            var sut = await repository.LoadAsync(size.Id);

            var sizeId = sut.Id;

            Assert.NotNull(sut);
            Assert.Equal("L", sut.Amount);

            await UpdateSizeAmount(sut.Id, "XL");

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal("XL", sut.Amount);
            Assert.Equal(sizeId, sut.Id);
        }

        [Fact]
        public async Task Remove_Size()
        {
            Size size = await CreateValidSize();

            var sut = await CheckIfSizeExists(size.Id);

            Assert.True(sut);

            await RemoveSize(size.Id);

            sut = await CheckIfSizeExists(size.Id);

            Assert.False(sut);
        }
    }
}
