using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;
using SplurgeStop.UI.WebApi.Controllers;
using country = SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;

namespace SplurgeStop.Integration.Tests.Helpers
{
    public static class CountryHelpers
    {
        public static async Task<Country> CreateValidCountry()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CountryRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CountryService(repository, unitOfWork);

            var command = new Commands.Create();
            command.Name = "Rapture";
            command.Id = null;

            var countryController = new CountryController(service);
            var country = await countryController.Post(command);

            return await repository.GetCountryAsync(country.Value.Id);
        }

        public static async Task<dynamic> CreateInvalidCountry()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CountryRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CountryService(repository, unitOfWork);

            var command = new Commands.Create();
            command.Id = null;

            // Create Store
            var countryController = new CountryController(service);
            return await countryController.Post(command);
        }

        public static async Task UpdateCountryName(CountryId id, string name)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CountryRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CountryService(repository, unitOfWork);
            var countryController = new CountryController(service);

            var updateCommand = new Commands.SetCountryName();
            updateCommand.Id = id;
            updateCommand.Name = name;

            await countryController.Put(updateCommand);
        }

        public static async Task<bool> CheckIfCountryExists(CountryId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CountryRepository(context);

            return await repository.ExistsAsync(id);
        }

        public static async Task RemoveCountry(CountryId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CountryRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CountryService(repository, unitOfWork);
            var countryController = new CountryController(service);

            var updateCommand = new Commands.DeleteCountry();
            updateCommand.Id = id;

            await countryController.DeleteCountry(updateCommand);
        }
    }
}
