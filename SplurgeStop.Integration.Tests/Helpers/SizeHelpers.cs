using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.UI.WebApi.Controllers;
using size = SplurgeStop.Domain.ProductProfile.SizeProfile;

namespace SplurgeStop.Integration.Tests.Helpers
{
    public static class SizeHelpers
    {
        public static async Task<size.Size> CreateValidSize()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new SizeRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new size.SizeService(repository, unitOfWork);

            var command = new size.Commands.Create
            {
                Amount = "L",
                Id = null
            };

            var sizeController = new SizeController(service);
            var size = await sizeController.Post(command);

            return await repository.GetAsync(size.Value.Id);
        }

        public static async Task<dynamic> CreateInvalidSize()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new SizeRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new size.SizeService(repository, unitOfWork);

            var command = new size.Commands.Create
            {
                Id = null
            };

            var sizeController = new SizeController(service);
            return await sizeController.Post(command);
        }

        public static async Task UpdateSizeAmount(size.SizeId id, string amount)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new SizeRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new size.SizeService(repository, unitOfWork);
            var sizeController = new SizeController(service);

            var updateCommand = new size.Commands.SetSizeAmount
            {
                Id = id,
                Amount = amount
            };

            await sizeController.Put(updateCommand);
        }

        public static async Task<bool> CheckIfSizeExists(size.SizeId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new SizeRepository(context);

            return await repository.ExistsAsync(id);
        }

        public static async Task RemoveSize(size.SizeId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new SizeRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new size.SizeService(repository, unitOfWork);
            var sizeController = new SizeController(service);

            var updateCommand = new size.Commands.DeleteSize
            {
                Id = id
            };

            await sizeController.DeleteSize(updateCommand);
        }
    }
}
