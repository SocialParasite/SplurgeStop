using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.Shared;
using static SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile.Commands;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile
{
    public sealed class CountryService : ICountryService
    {
        private readonly ICountryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(ICountryRepository repository,
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
                SetCountryName cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateCountryName(cmd.Name)),
                DeleteCountry cmd => HandleUpdateAsync(cmd.Id, _ => this._repository.RemoveCountryAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await _repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var newCountry = Country.Create(cmd.Id, cmd.Name);

            await _repository.AddCountryAsync(newCountry);

            if (newCountry.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid countryId, Func<Country, Task> operation)
        {
            var country = await _repository.LoadCountryAsync(countryId);

            if (country == null)
                throw new InvalidOperationException($"Entity with id {countryId} cannot be found");

            await operation(country);

            if (country.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid countryId, Action<Country> operation)
        {
            var country = await _repository.LoadCountryAsync(countryId);

            if (country == null)
                throw new InvalidOperationException($"Entity with id {countryId} cannot be found");

            operation(country);

            if (country.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<CountryDto>> GetAllCountryDtoAsync()
        {
            return await _repository.GetAllCountryDtoAsync();
        }

        public async Task<Country> GetCountryAsync(CountryId id)
        {
            return await _repository.GetCountryAsync(id);
        }
    }
}
