using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Domain.CountryProfile;
using SplurgeStop.Domain.CountryProfile.DTO;
using SplurgeStop.Domain.DA_Interfaces;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class CountryRepository : ICountryRepository
    {
        private readonly SplurgeStopDbContext context;

        public CountryRepository(SplurgeStopDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsAsync(CountryId id)
        {
            return await context.Countries.FindAsync(id) != null;
        }

        public async Task<Country> LoadCountryAsync(CountryId id)
        {
            return await context.Countries.FindAsync(id);
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await context.Countries
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<CountryDto>> GetAllCountryDtoAsync()
        {
            return await context.Countries
                    .Select(r => new CountryDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<Country> GetCountryAsync(CountryId id)
        {
            return await context.Countries
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCountryAsync(Country country)
        {
            await context.Countries.AddAsync(country);
        }

        public async Task RemoveCountryAsync(CountryId id)
        {
            var country = await context.Countries.FindAsync(id);

            if (country != null)
                context.Countries.Remove(country);
        }
    }
}
