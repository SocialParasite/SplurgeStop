using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.UI.WebApi.Controllers;

namespace SplurgeStop.Integration.Tests.Helpers
{
    public static class CityHelpers
    {
        public static async Task<City> CreateValidCity()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CityRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CityService(repository, unitOfWork);

            var command = new Commands.Create
            {
                Name = "New Mansester",
                Id = null
            };

            var cityController = new CityController(service);
            var city = await cityController.Post(command);

            return await repository.GetAsync(city.Value.Id);
        }

        public static async Task<dynamic> CreateInvalidCity()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CityRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CityService(repository, unitOfWork);

            var command = new Commands.Create { Id = null };

            // Create Store
            var cityController = new CityController(service);
            return await cityController.Post(command);
        }

        public static async Task UpdateCityName(CityId id, string name)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CityRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CityService(repository, unitOfWork);
            var cityController = new CityController(service);

            var updateCommand = new Commands.SetCityName { Id = id, Name = name };

            await cityController.Put(updateCommand);
        }

        public static async Task<bool> CheckIfCityExists(CityId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CityRepository(context);

            return await repository.ExistsAsync(id);
        }

        public static async Task RemoveCity(CityId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CityRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CityService(repository, unitOfWork);
            var cityController = new CityController(service);

            var updateCommand = new Commands.DeleteCity
            {
                Id = id
            };

            await cityController.DeleteCity(updateCommand);
        }
    }
}
