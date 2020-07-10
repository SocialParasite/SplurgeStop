using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;
using SplurgeStop.UI.WebApi.Controllers;
using product = SplurgeStop.Domain.ProductProfile;

namespace SplurgeStop.Integration.Tests.Helpers
{
    public static class ProductHelpers
    {
        public static async Task<Product> CreateValidProduct()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new ProductService(repository, unitOfWork);

            var newBrand = await BrandHelpers.CreateValidBrand();

            var command = new product.Commands.Create
            {
                Name = "New product",
                Id = null,
                BrandId = newBrand.Id
            };


            // Create product
            var productController = new ProductController(service);
            await productController.Post(command);


            return await repository.GetProductFullAsync(command.Id);
        }

        public static async Task<dynamic> CreateInvalidProduct()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new ProductService(repository, unitOfWork);

            var command = new product.Commands.Create
            {
                Id = null
            };

            // Create product
            var productController = new ProductController(service);
            return await productController.Post(command);
        }

        public static async Task UpdateProductName(ProductId id, string name, BrandId brandId)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new ProductService(repository, unitOfWork);
            var productController = new ProductController(service);

            var updateCommand = new product.Commands.UpdateProduct
            {
                Id = id,
                Name = name,
                BrandId = brandId
            };

            await productController.Put(updateCommand);
        }

        public static async Task UpdateProductBrand(ProductId id, Brand brand)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new ProductService(repository, unitOfWork);
            var productController = new ProductController(service);

            var updateCommand = new product.Commands.ChangeBrand
            {
                Id = id,
                BrandId = brand.Id
            };

            await productController.Put(updateCommand);
        }

        public static async Task UpdateProductType(ProductId id, ProductType productType)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new ProductService(repository, unitOfWork);
            var productController = new ProductController(service);

            var updateCommand = new product.Commands.ChangeProductType
            {
                Id = id,
                ProductTypeId = productType.Id
            };

            await productController.Put(updateCommand);
        }

        public static async Task UpdateProductSize(ProductId id, Size size)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new ProductService(repository, unitOfWork);
            var productController = new ProductController(service);

            var updateCommand = new product.Commands.ChangeSize
            {
                Id = id,
                SizeId = size.Id
            };

            await productController.Put(updateCommand);
        }

        public static async Task RemoveProduct(ProductId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new ProductService(repository, unitOfWork);
            var productController = new ProductController(service);

            var updateCommand = new product.Commands.DeleteProduct
            {
                Id = id
            };

            await productController.DeleteProduct(updateCommand);
        }

        public static async Task<bool> CheckIfProductExists(ProductId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductRepository(context);

            return await repository.ExistsAsync(id);
        }

    }
}
