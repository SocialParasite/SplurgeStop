using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.Shared;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;
using static SplurgeStop.Domain.StoreProfile.LocationProfile.Commands;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile
{
    public sealed class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public LocationService(ILocationRepository repository,
                           IUnitOfWork unitOfWork)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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
                DeleteLocation cmd => HandleUpdateAsync(cmd.Id, _ => this._repository.RemoveAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task ChangeCountryAsync(Location location, CountryId countryId)
        {

            await _repository.ChangeCountry(location, countryId);

        }

        private async Task ChangeCityAsync(Location location, CityId cityId)
        {

            await _repository.ChangeCity(location, cityId);

        }

        private async Task HandleCreate(Create cmd)
        {
            if (await _repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var city = await _repository.GetCityAsync(cmd.CityId);
            var country = await _repository.GetCountryAsync(cmd.CountryId);

            var newLocation = Location.Create(cmd.Id, city, country);

            await _repository.AddAsync(newLocation);

            if (newLocation.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid locationId, Func<Location, Task> operation)
        {
            var location = await _repository.LoadAsync(locationId);

            if (location == null)
                throw new InvalidOperationException($"Entity with id {locationId} cannot be found");

            await operation(location);

            if (location.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<LocationDto>> GetAllLocationDtoAsync()
        {
            return await _repository.GetAllDtoAsync();
        }

        public async Task<Location> GetLocationAsync(LocationId id)
        {
            return await _repository.GetAsync(id);
        }
    }
}
