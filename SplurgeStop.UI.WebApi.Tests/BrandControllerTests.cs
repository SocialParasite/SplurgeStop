using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuidHelpers;
using Moq;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.DTO;
using SplurgeStop.Domain.Shared;
using SplurgeStop.UI.WebApi.Controllers;
using Xunit;

namespace SplurgeStop.UI.WebApi.Tests
{
    [Trait("Unit tests", "WebApi/BrandController")]
    public sealed class BrandControllerTests
    {
        private static List<BrandDto> MockBrands()
        {
            var mockBrands = new List<BrandDto>();

            for (int i = 1; i <= 10; i++)
            {
                mockBrands.Add(new BrandDto
                {
                    Id = SequentialGuid.NewSequentialGuid(),
                    Name = $"Brand-{i}",
                });
            }

            return mockBrands;
        }

        [Fact]
        public async Task Get_All_Brands()
        {
            List<BrandDto> mockBrands = MockBrands();

            var mockRepository = new Mock<IBrandRepository>();
            mockRepository.Setup(repo => repo.GetAllBrandDtoAsync())
                .Returns(() => Task.FromResult(mockBrands.AsEnumerable()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var brandService = new BrandService(mockRepository.Object, mockUnitOfWork.Object);

            var brandController = new BrandController(brandService);
            var result = await brandController.GetBrands();

            Assert.Equal(10, result.Count());
            mockRepository.Verify(mock => mock.GetAllBrandDtoAsync(), Times.Once());
        }


        [Fact]
        public async Task Valid_Id_Returns_Brand()
        {
            var mockBrand = new Mock<Brand>();
            var id = Guid.NewGuid();

            var mockBrandService = new Mock<IBrandService>();
            mockBrandService.Setup(s => s.GetBrandAsync(id))
                .Returns(() => Task.FromResult(mockBrand.Object));

            var brandController = new BrandController(mockBrandService.Object);
            var result = await brandController.GetBrand(id);

            mockBrandService.Verify(mock => mock.GetBrandAsync(id), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Invalid_Id_Returns_Null()
        {
            var mockBrand = new Mock<Brand>();
            var id = Guid.NewGuid();

            var mockBrandService = new Mock<IBrandService>();
            mockBrandService.Setup(s => s.GetBrandAsync(Guid.NewGuid()))
                .Returns(() => Task.FromResult(mockBrand.Object));

            var brandController = new BrandController(mockBrandService.Object);

            var result = await brandController.GetBrand(id);
            Assert.Null(result);
        }
    }
}
