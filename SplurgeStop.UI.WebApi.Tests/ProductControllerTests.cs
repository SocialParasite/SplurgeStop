using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuidHelpers;
using Moq;
using SplurgeStop.Domain;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.DTO;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.UI.WebApi.Controllers;
using Xunit;

namespace SplurgeStop.UI.WebApi.Tests
{
    [Trait("Unit tests", "WebApi/ProductController")]
    public sealed class ProductControllerTests
    {
        private static List<ProductDto> MockProducts()
        {
            var mockProducts = new List<ProductDto>();

            for (int i = 1; i <= 10; i++)
            {
                mockProducts.Add(new ProductDto
                {
                    Id = SequentialGuid.NewSequentialGuid(),
                    Name = $"Product-{i}",
                });
            }

            return mockProducts;
        }

        [Fact]
        public async Task Get_All_Products()
        {
            List<ProductDto> mockProducts = MockProducts();

            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(repo => repo.GetAllProductDtoAsync())
                .Returns(() => Task.FromResult(mockProducts.AsEnumerable()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var productService = new ProductService(mockRepository.Object, mockUnitOfWork.Object);

            var productController = new ProductController(productService);
            var result = await productController.GetProducts();

            Assert.Equal(10, result.Count());
            mockRepository.Verify(mock => mock.GetAllProductDtoAsync(), Times.Once());
        }


        [Fact]
        public async Task Valid_Id_Returns_Product()
        {
            var mockRepository = new Mock<IProductRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockProduct = new Mock<Product>();
            var id = Guid.NewGuid();

            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(s => s.GetProductAsync(id))
                .Returns(() => Task.FromResult(mockProduct.Object));

            var productController = new ProductController(mockProductService.Object);
            var result = await productController.GetProduct(id);

            mockProductService.Verify(mock => mock.GetProductAsync(id), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Invalid_Id_Returns_Null()
        {
            var mockRepository = new Mock<IProductRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockProduct = new Mock<Product>();
            var id = Guid.NewGuid();

            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(s => s.GetProductAsync(Guid.NewGuid()))
                .Returns(() => Task.FromResult(mockProduct.Object));

            var productController = new ProductController(mockProductService.Object);

            var result = await productController.GetProduct(id);
            Assert.Null(result);
        }
    }
}
