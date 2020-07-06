using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.UI.WebApi.Controllers;
using city = SplurgeStop.Domain.CityProfile;

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

            var command = new Commands.Create();
            command.Name = "New Mansester";
            command.Id = null;

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

            var command = new Commands.Create();
            command.Id = null;

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

            var updateCommand = new Commands.SetCityName();
            updateCommand.Id = id;
            updateCommand.Name = name;

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

            var updateCommand = new Commands.DeleteCity();
            updateCommand.Id = id;

            await cityController.DeleteCity(updateCommand);
        }
    }
}
