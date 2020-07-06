using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.UI.WebApi.Controllers;
using SplurgeStop.Domain.ProductProfile.TypeProfile;

namespace SplurgeStop.Integration.Tests.Helpers
{
    public static class ProductTypeHelpers
    {
        public static async Task<ProductType> CreateValidProductType()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductTypeRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new ProductTypeService(repository, unitOfWork);

            var command = new Commands.Create();
            command.Name = "trousers";
            command.Id = null;

            var productTypeController = new ProductTypeController(service);
            var productType = await productTypeController.Post(command);

            return await repository.GetAsync(productType.Value.Id);
        }

        public static async Task<dynamic> CreateInvalidProductType()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductTypeRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new ProductTypeService(repository, unitOfWork);

            var command = new Commands.Create();
            command.Id = null;

            // Create ProductType
            var productTypeController = new ProductTypeController(service);
            return await productTypeController.Post(command);
        }

        public static async Task UpdateProductTypeName(ProductTypeId id, string name)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductTypeRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new ProductTypeService(repository, unitOfWork);
            var productTypeController = new ProductTypeController(service);

            var updateCommand = new Commands.SetProductTypeName();
            updateCommand.Id = id;
            updateCommand.Name = name;

            await productTypeController.Put(updateCommand);
        }

        public static async Task<bool> CheckIfProductTypeExists(ProductTypeId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductTypeRepository(context);

            return await repository.ExistsAsync(id);
        }

        public static async Task RemoveProductType(ProductTypeId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new ProductTypeRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new ProductTypeService(repository, unitOfWork);
            var productTypeController = new ProductTypeController(service);

            var updateCommand = new Commands.DeleteProductType();
            updateCommand.Id = id;

            await productTypeController.DeleteProductType(updateCommand);
        }
    }
}
