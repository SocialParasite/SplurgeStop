using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
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

            var command = new store.Commands.Create
            {
                Name = "New store",
                Id = null,
                LocationId = newLocation.Id
            };


            // Create Store
            var storeController = new StoreController(service);
            await storeController.Post(command);


            return await repository.GetAsync(command.Id);
        }

        public static async Task<dynamic> CreateInvalidStore()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);

            var command = new store.Commands.Create
            {
                Id = null
            };

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

            var updateCommand = new store.Commands.UpdateStore
            {
                Id = id,
                Name = name,
                LocationId = locationId
            };

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

            var updateCommand = new store.Commands.ChangeLocation
            {
                Id = id,
                Location = location
            };

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

            var updateCommand = new store.Commands.DeleteStore
            {
                Id = id
            };

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
