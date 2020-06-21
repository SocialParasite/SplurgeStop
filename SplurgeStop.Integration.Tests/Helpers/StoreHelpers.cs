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
        public async static Task<Store> CreateValidStore()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);

            var command = new store.Commands.Create();
            command.Name = "New store";
            command.Id = null;

            // Create Store
            var storeController = new StoreController(service);
            var storeId = await storeController.Post(command);

            // Update store name
            var updateCommand = new store.Commands.SetStoreName();
            updateCommand.Id = storeId.Value.Id;
            updateCommand.Name = "Test market";

            await storeController.Put(updateCommand);

            return await repository.GetStoreFullAsync(command.Id);
        }

        public async static Task<dynamic> CreateInvalidStore()
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

        public async static Task UpdateStoreName(StoreId id, string name)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);
            var storeController = new StoreController(service);

            var updateCommand = new store.Commands.SetStoreName();
            updateCommand.Id = id;
            updateCommand.Name = name;

            await storeController.Put(updateCommand);
        }

        public async static Task UpdateStoreLocation(StoreId id, Location location)
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

        public async static Task RemoveStore(StoreId id)
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

        public async static Task<bool> CheckIfStoreExists(StoreId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);

            return await repository.ExistsAsync(id);
        }

    }
}
