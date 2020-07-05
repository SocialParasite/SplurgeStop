using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.Shared;
using static SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile.Commands;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile
{
    public sealed class CityService : ICityService
    {
        private readonly ICityRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CityService(ICityRepository repository,
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
                SetCityName cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateCityName(cmd.Name)),
                DeleteCity cmd => HandleUpdateAsync(cmd.Id, _ => this._repository.RemoveCityAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await _repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var newCity = City.Create(cmd.Id, cmd.Name);

            await _repository.AddCityAsync(newCity);

            if (newCity.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid cityId, Func<City, Task> operation)
        {
            var city = await _repository.LoadCityAsync(cityId);

            if (city == null)
                throw new InvalidOperationException($"Entity with id {cityId} cannot be found");

            await operation(city);

            if (city.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid cityId, Action<City> operation)
        {
            var city = await _repository.LoadCityAsync(cityId);

            if (city == null)
                throw new InvalidOperationException($"Entity with id {cityId} cannot be found");

            operation(city);

            if (city.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<CityDto>> GetAllCityDtoAsync()
        {
            return await _repository.GetAllCityDtoAsync();
        }

        public async Task<City> GetCityAsync(CityId id)
        {
            return await _repository.GetCityAsync(id);
        }
    }
}
