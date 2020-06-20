using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.LocationProfile;
using location = SplurgeStop.Domain.LocationProfile;
using SplurgeStop.UI.WebApi.Controllers;
using SplurgeStop.Domain.CountryProfile;
using System;
using SplurgeStop.Domain.CityProfile;

namespace SplurgeStop.Integration.Tests.Helpers
{
    public static class LocationHelpers
    {
        public async static Task<Location> CreateValidLocation()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new LocationRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new LocationService(repository, unitOfWork);

            var city = await CityHelpers.CreateValidCity();
            var country = await CountryHelpers.CreateValidCountry();

            var command = new location.Commands.Create();
            command.Id = null;
            command.CityId = city.Id;
            command.CountryId = country.Id;

            var locationController = new LocationController(service);
            var location = await locationController.Post(command);

            return await repository.GetLocationAsync(location.Value.Id);
        }

        public async static Task<dynamic> CreateInvalidLocation(string invalidProp)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new LocationRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new LocationService(repository, unitOfWork);

            var command = new location.Commands.Create();
            command.Id = null;
            command.CityId = invalidProp == "CityId" ? default : Guid.NewGuid();
            command.CountryId = invalidProp == "CountryId" ? default : Guid.NewGuid();

            var locationController = new LocationController(service);
            return await locationController.Post(command);
        }

        public async static Task UpdateLocationCity(LocationId id, City city)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new LocationRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new LocationService(repository, unitOfWork);
            var locationController = new LocationController(service);

            var updateCommand = new location.Commands.ChangeCity();
            updateCommand.Id = id;
            updateCommand.City = city;

            await locationController.Put(updateCommand);
        }

        public async static Task UpdateLocationCountry(LocationId id, Country country)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new LocationRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new LocationService(repository, unitOfWork);
            var locationController = new LocationController(service);

            var updateCommand = new location.Commands.ChangeCountry();
            updateCommand.Id = id;
            updateCommand.Country = country;

            await locationController.Put(updateCommand);
        }

        public async static Task<bool> CheckIfLocationExists(LocationId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new LocationRepository(context);

            return await repository.ExistsAsync(id);
        }

        public async static Task RemoveLocation(LocationId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new LocationRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new LocationService(repository, unitOfWork);
            var locationController = new LocationController(service);

            var updateCommand = new location.Commands.DeleteLocation();
            updateCommand.Id = id;

            await locationController.DeleteLocation(updateCommand);
        }
    }
}
