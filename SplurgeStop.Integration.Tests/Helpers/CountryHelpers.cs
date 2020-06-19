using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.CountryProfile;
using SplurgeStop.UI.WebApi.Controllers;
using country = SplurgeStop.Domain.CountryProfile;

namespace SplurgeStop.Integration.Tests.Helpers
{
    public static class CountryHelpers
    {
        public async static Task<Country> CreateValidCountry()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CountryRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CountryService(repository, unitOfWork);

            var command = new country.Commands.Create();
            command.Name = "Rapture";
            command.Id = null;

            var countryController = new CountryController(service);
            var country = await countryController.Post(command);

            return await repository.GetCountryAsync(country.Value.Id);
        }

        public async static Task<dynamic> CreateInvalidCountry()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CountryRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CountryService(repository, unitOfWork);

            var command = new country.Commands.Create();
            command.Id = null;

            // Create Store
            var countryController = new CountryController(service);
            return await countryController.Post(command);
        }

        public async static Task UpdateCountryName(CountryId id, string name)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CountryRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CountryService(repository, unitOfWork);
            var countryController = new CountryController(service);

            var updateCommand = new country.Commands.SetCountryName();
            updateCommand.Id = id;
            updateCommand.Name = name;

            await countryController.Put(updateCommand);
        }

        public async static Task<bool> CheckIfCountryExists(CountryId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CountryRepository(context);

            return await repository.ExistsAsync(id);
        }

        public async static Task RemoveCountry(CountryId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CountryRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CountryService(repository, unitOfWork);
            var countryController = new CountryController(service);

            var updateCommand = new country.Commands.DeleteCountry();
            updateCommand.Id = id;

            await countryController.DeleteCountry(updateCommand);
        }
    }
}
