using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuidHelpers;
using Moq;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.ProductProfile.DTO;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.Shared;
using SplurgeStop.UI.WebApi.Controllers;
using Xunit;

namespace SplurgeStop.UI.WebApi.Tests
{
    [Trait("Unit tests", "WebApi/SizeController")]
    public sealed class SizeControllerTests
    {
        private static List<SizeDto> MockSizes()
        {
            var mockSizes = new List<SizeDto>();

            for (int i = 1; i <= 10; i++)
            {
                mockSizes.Add(new SizeDto
                {
                    Id = SequentialGuid.NewSequentialGuid(),
                    Amount = $"Size-{i}",
                });
            }

            return mockSizes;
        }

        [Fact]
        public async Task Get_All_Sizes()
        {
            List<SizeDto> mockSizes = MockSizes();

            var mockRepository = new Mock<ISizeRepository>();
            mockRepository.Setup(repo => repo.GetAllSizeDtoAsync())
                .Returns(() => Task.FromResult(mockSizes.AsEnumerable()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var sizeService = new SizeService(mockRepository.Object, mockUnitOfWork.Object);

            var sizeController = new SizeController(sizeService);
            var result = await sizeController.GetSizes();

            Assert.Equal(10, result.Count());
            mockRepository.Verify(mock => mock.GetAllSizeDtoAsync(), Times.Once());
        }


        [Fact]
        public async Task Valid_Id_Returns_Size()
        {
            var mockSize = new Mock<Size>();
            var id = Guid.NewGuid();

            var mockSizeService = new Mock<ISizeService>();
            mockSizeService.Setup(s => s.GetSizeAsync(id))
                .Returns(() => Task.FromResult(mockSize.Object));

            var sizeController = new SizeController(mockSizeService.Object);
            var result = await sizeController.GetSize(id);

            mockSizeService.Verify(mock => mock.GetSizeAsync(id), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Invalid_Id_Returns_Null()
        {
            var mockSize = new Mock<Size>();
            var id = Guid.NewGuid();

            var mockSizeService = new Mock<ISizeService>();
            mockSizeService.Setup(s => s.GetSizeAsync(Guid.NewGuid()))
                .Returns(() => Task.FromResult(mockSize.Object));

            var sizeController = new SizeController(mockSizeService.Object);

            var result = await sizeController.GetSize(id);
            Assert.Null(result);
        }
    }
}
