using System;
using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;
using SplurgeStop.UI.WebApi.Controllers;
using Commands = SplurgeStop.Domain.StoreProfile.LocationProfile.Commands;
using location = SplurgeStop.Domain.StoreProfile.LocationProfile;

namespace SplurgeStop.Integration.Tests.Helpers
{
    public static class LocationHelpers
    {
        public static async Task<Location> CreateValidLocation()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new LocationRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new LocationService(repository, unitOfWork);

            var city = await CityHelpers.CreateValidCity();
            var country = await CountryHelpers.CreateValidCountry();

            var command = new Commands.Create { Id = null, CityId = city.Id, CountryId = country.Id };

            var locationController = new LocationController(service);
            var location = await locationController.Post(command);

            return await repository.GetAsync(location.Value.Id);
        }

        public static async Task<dynamic> CreateInvalidLocation(string invalidProp)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new LocationRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new LocationService(repository, unitOfWork);

            var command = new Commands.Create
            {
                Id = null,
                CityId = invalidProp == "CityId" ? default : Guid.NewGuid(),
                CountryId = invalidProp == "CountryId" ? default : Guid.NewGuid()
            };

            var locationController = new LocationController(service);
            return await locationController.Post(command);
        }

        public static async Task UpdateLocationCity(LocationId id, City city)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new LocationRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new LocationService(repository, unitOfWork);
            var locationController = new LocationController(service);

            var updateCommand = new Commands.ChangeCity
            {
                Id = id,
                City = city
            };

            await locationController.Put(updateCommand);
        }

        public static async Task UpdateLocationCountry(LocationId id, Country country)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new LocationRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new LocationService(repository, unitOfWork);
            var locationController = new LocationController(service);

            var updateCommand = new Commands.ChangeCountry
            {
                Id = id,
                Country = country
            };

            await locationController.Put(updateCommand);
        }

        public static async Task<bool> CheckIfLocationExists(LocationId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new LocationRepository(context);

            return await repository.ExistsAsync(id);
        }

        public static async Task RemoveLocation(LocationId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new LocationRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new LocationService(repository, unitOfWork);
            var locationController = new LocationController(service);

            var updateCommand = new Commands.DeleteLocation
            {
                Id = id
            };

            await locationController.DeleteLocation(updateCommand);
        }
    }
}
