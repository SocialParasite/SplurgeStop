using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.CountryProfile.DTO;
using SplurgeStop.Domain.DA_Interfaces;
using static SplurgeStop.Domain.CountryProfile.Commands;

namespace SplurgeStop.Domain.CountryProfile
{
    public sealed class CountryService : ICountryService
    {
        private readonly ICountryRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public CountryService(ICountryRepository repository,
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
                SetCountryName cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateCountryName(cmd.Name)),
                DeleteCountry cmd => HandleUpdateAsync(cmd.Id, _ => this.repository.RemoveCountryAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var newCountry = Country.Create(cmd.Id, cmd.Name);

            await repository.AddCountryAsync(newCountry);

            if (newCountry.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task HandleUpdateAsync(Guid countryId, Func<Country, Task> operation)
        {
            var country = await repository.LoadCountryAsync(countryId);

            if (country == null)
                throw new InvalidOperationException($"Entity with id {countryId} cannot be found");

            await operation(country);

            if (country.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid countryId, Action<Country> operation)
        {
            var country = await repository.LoadCountryAsync(countryId);

            if (country == null)
                throw new InvalidOperationException($"Entity with id {countryId} cannot be found");

            operation(country);

            if (country.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<CountryDto>> GetAllCountryDtoAsync()
        {
            return await repository.GetAllCountryDtoAsync();
        }

        public async Task<Country> GetCountryAsync(CountryId id)
        {
            return await repository.GetCountryAsync(id);
        }
    }
}
