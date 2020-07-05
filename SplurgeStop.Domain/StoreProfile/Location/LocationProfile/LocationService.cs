using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.LocationProfile.DTO;
using SplurgeStop.Domain.DA_Interfaces;
using static SplurgeStop.Domain.LocationProfile.Commands;
using SplurgeStop.Domain.CountryProfile;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.Shared;

namespace SplurgeStop.Domain.LocationProfile
{
    public sealed class LocationService : ILocationService
    {
        private readonly ILocationRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public LocationService(ILocationRepository repository,
                           IUnitOfWork unitOfWork)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task Handle(object command)
        {
            return command switch
            {
                Create cmd => HandleCreate(cmd),
                ChangeCity cmd
                    => HandleUpdateAsync(cmd.Id, async c => await ChangeCityAsync(c, cmd.City.Id)),
                ChangeCountry cmd
                    => HandleUpdateAsync(cmd.Id, async c => await ChangeCountryAsync(c, cmd.Country.Id)),
                DeleteLocation cmd => HandleUpdateAsync(cmd.Id, _ => this.repository.RemoveLocationAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task ChangeCountryAsync(Location location, CountryId countryId)
        {

            await repository.ChangeCountry(location, countryId);

        }

        private async Task ChangeCityAsync(Location location, CityId cityId)
        {

            await repository.ChangeCity(location, cityId);

        }

        private async Task HandleCreate(Create cmd)
        {
            if (await repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var city = await repository.GetCityAsync(cmd.CityId);
            var country = await repository.GetCountryAsync(cmd.CountryId);

            var newLocation = Location.Create(cmd.Id, city, country);

            await repository.AddLocationAsync(newLocation);

            if (newLocation.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid locationId, Func<Location, Task> operation)
        {
            var location = await repository.LoadLocationAsync(locationId);

            if (location == null)
                throw new InvalidOperationException($"Entity with id {locationId} cannot be found");

            await operation(location);

            if (location.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<LocationDto>> GetAllLocationDtoAsync()
        {
            return await repository.GetAllLocationDtoAsync();
        }

        public async Task<Location> GetLocationAsync(LocationId id)
        {
            return await repository.GetLocationAsync(id);
        }
    }
}
