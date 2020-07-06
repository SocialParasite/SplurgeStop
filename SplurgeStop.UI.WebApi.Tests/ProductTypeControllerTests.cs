using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuidHelpers;
using Moq;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.ProductProfile.TypeProfile;
using SplurgeStop.Domain.Shared;
using SplurgeStop.UI.WebApi.Controllers;
using Xunit;

namespace SplurgeStop.UI.WebApi.Tests
{
    [Trait("Unit tests", "WebApi/ProductTypeController")]
    public sealed class ProductTypeControllerTests
    {
        private static List<ProductTypeDto> MockProductTypes()
        {
            var mockProductTypes = new List<ProductTypeDto>();

            for (int i = 1; i <= 10; i++)
            {
                mockProductTypes.Add(new ProductTypeDto
                {
                    Id = SequentialGuid.NewSequentialGuid(),
                    Name = $"ProductType-{i}",
                });
            }

            return mockProductTypes;
        }

        [Fact]
        public async Task Get_All_ProductTypes()
        {
            List<ProductTypeDto> mockProductTypes = MockProductTypes();

            var mockRepository = new Mock<IRepository<ProductType, ProductTypeDto, ProductTypeId>>();
            mockRepository.Setup(repo => repo.GetAllDtoAsync())
                .Returns(() => Task.FromResult(mockProductTypes.AsEnumerable()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var productTypeService = new ProductTypeService(mockRepository.Object, mockUnitOfWork.Object);

            var productTypeController = new ProductTypeController(productTypeService);
            var result = await productTypeController.GetProductTypes();

            Assert.Equal(10, result.Count());
            mockRepository.Verify(mock => mock.GetAllDtoAsync(), Times.Once());
        }


        [Fact]
        public async Task Valid_Id_Returns_ProductType()
        {
            var mockProductType = new Mock<ProductType>();
            var id = Guid.NewGuid();

            var mockProductTypeService = new Mock<IProductTypeService>();
            mockProductTypeService.Setup(s => s.GetProductTypeAsync(id))
                .Returns(() => Task.FromResult(mockProductType.Object));

            var productTypeController = new ProductTypeController(mockProductTypeService.Object);
            var result = await productTypeController.GetProductType(id);

            mockProductTypeService.Verify(mock => mock.GetProductTypeAsync(id), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Invalid_Id_Returns_Null()
        {
            var mockProductType = new Mock<ProductType>();
            var id = Guid.NewGuid();

            var mockProductTypeService = new Mock<IProductTypeService>();
            mockProductTypeService.Setup(s => s.GetProductTypeAsync(Guid.NewGuid()))
                .Returns(() => Task.FromResult(mockProductType.Object));

            var productTypeController = new ProductTypeController(mockProductTypeService.Object);

            var result = await productTypeController.GetProductType(id);
            Assert.Null(result);
        }
    }
}
