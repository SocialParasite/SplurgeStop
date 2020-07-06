using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuidHelpers;
using Moq;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.Shared;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.UI.WebApi.Controllers;
using Xunit;

namespace SplurgeStop.UI.WebApi.Tests
{
    [Trait("Unit tests", "WebApi/StoreController")]
    public sealed class StoreControllerTests
    {
        private static List<StoreStripped> MockStores()
        {
            var mockStores = new List<StoreStripped>();

            for (int i = 1; i <= 10; i++)
            {
                mockStores.Add(new StoreStripped
                {
                    Id = SequentialGuid.NewSequentialGuid(),
                    Name = $"Store-{i}",
                });
            }

            return mockStores;
        }

        [Fact]
        public async Task Get_All_Stores()
        {
            List<StoreStripped> mockStores = MockStores();

            var mockRepository = new Mock<IStoreRepository>();
            mockRepository.Setup(repo => repo.GetAllDtoAsync())
                .Returns(() => Task.FromResult(mockStores.AsEnumerable()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var storeService = new StoreService(mockRepository.Object, mockUnitOfWork.Object);

            var storeController = new StoreController(storeService);
            var result = await storeController.GetStores();

            Assert.Equal(10, result.Count());
            mockRepository.Verify(mock => mock.GetAllDtoAsync(), Times.Once());
        }


        [Fact]
        public async Task Valid_Id_Returns_Store()
        {
            var mockStore = new Mock<Store>();
            var id = Guid.NewGuid();

            var mockStoreService = new Mock<IStoreService>();
            mockStoreService.Setup(s => s.GetDetailedStore(id))
                .Returns(() => Task.FromResult(mockStore.Object));

            var storeController = new StoreController(mockStoreService.Object);
            var result = await storeController.GetStore(id);

            mockStoreService.Verify(mock => mock.GetDetailedStore(id), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Invalid_Id_Returns_Null()
        {
            var mockStore = new Mock<Store>();
            var id = Guid.NewGuid();

            var mockStoreService = new Mock<IStoreService>();
            mockStoreService.Setup(s => s.GetDetailedStore(Guid.NewGuid()))
                .Returns(() => Task.FromResult(mockStore.Object));

            var storeController = new StoreController(mockStoreService.Object);

            var result = await storeController.GetStore(id);
            Assert.Null(result);
        }
    }
}
