using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.LocationProfile;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.UI.WebApi.Controllers;
using store = SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Integration.Tests.Helpers
{
    public static class StoreHelpers
    {
        public static async Task<Store> CreateValidStore()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);

            var newLocation = await LocationHelpers.CreateValidLocation();

            var command = new store.Commands.Create();
            command.Name = "New store";
            command.Id = null;
            command.LocationId = newLocation.Id;


            // Create Store
            var storeController = new StoreController(service);
            await storeController.Post(command);


            return await repository.GetStoreFullAsync(command.Id);
        }

        public static async Task<dynamic> CreateInvalidStore()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);

            var command = new store.Commands.Create();
            command.Id = null;

            // Create Store
            var storeController = new StoreController(service);
            return await storeController.Post(command);
        }

        public static async Task UpdateStoreName(StoreId id, string name, LocationId locationId)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);
            var storeController = new StoreController(service);

            var updateCommand = new store.Commands.UpdateStore();
            updateCommand.Id = id;
            updateCommand.Name = name;
            updateCommand.LocationId = locationId;

            await storeController.Put(updateCommand);
        }

        public static async Task UpdateStoreLocation(StoreId id, Location location)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);
            var storeController = new StoreController(service);

            var updateCommand = new store.Commands.ChangeLocation();
            updateCommand.Id = id;
            updateCommand.Location = location;

            await storeController.Put(updateCommand);
        }

        public static async Task RemoveStore(StoreId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);
            var storeController = new StoreController(service);

            var updateCommand = new store.Commands.DeleteStore();
            updateCommand.Id = id;

            await storeController.DeleteStore(updateCommand);
        }

        public static async Task<bool> CheckIfStoreExists(StoreId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);

            return await repository.ExistsAsync(id);
        }

    }
}
