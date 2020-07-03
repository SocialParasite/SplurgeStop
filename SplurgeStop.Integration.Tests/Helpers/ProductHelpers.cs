using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
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

            var command = new product.Commands.Create();
            command.Name = "New product";
            command.Id = null;
            command.BrandId = newBrand.Id;


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

            var command = new product.Commands.Create();
            command.Id = null;

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

            var updateCommand = new product.Commands.UpdateProduct();
            updateCommand.Id = id;
            updateCommand.Name = name;
            updateCommand.BrandId = brandId;

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

            var updateCommand = new product.Commands.ChangeBrand();
            updateCommand.Id = id;
            updateCommand.BrandId = brand.Id;

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

            var updateCommand = new product.Commands.DeleteProduct();
            updateCommand.Id = id;

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
