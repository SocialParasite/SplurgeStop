using System;
using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.UI.WebApi.Controllers;

namespace SplurgeStop.Integration.Tests.Helpers
{
    public static class BrandHelpers
    {
        public static async Task<Brand> CreateValidBrand()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new BrandRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new BrandService(repository, unitOfWork);

            var command = new Commands.Create();
            command.Name = "Levi's";
            command.Id = null;

            var brandController = new BrandController(service);
            var brand = await brandController.Post(command);

            return await repository.GetAsync(brand.Value.Id);
        }

        public static async Task<dynamic> CreateInvalidBrand()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new BrandRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new BrandService(repository, unitOfWork);

            var command = new Commands.Create();
            command.Id = null;

            var brandController = new BrandController(service);

            return await brandController.Post(command);
        }

        public static async Task UpdateBrandName(BrandId id, string name)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new BrandRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new BrandService(repository, unitOfWork);
            var brandController = new BrandController(service);

            var updateCommand = new Commands.SetBrandName();
            updateCommand.Id = id;
            updateCommand.Name = name;

            await brandController.Put(updateCommand);
        }

        public static async Task<bool> CheckIfBrandExists(BrandId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new BrandRepository(context);

            return await repository.ExistsAsync(id);
        }

        public static async Task RemoveBrand(BrandId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new BrandRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new BrandService(repository, unitOfWork);
            var brandController = new BrandController(service);

            var updateCommand = new Commands.DeleteBrand();
            updateCommand.Id = id;

            await brandController.DeleteBrand(updateCommand);
        }
    }
}
