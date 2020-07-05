using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Domain.CountryProfile.DTO;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class CountryRepository : ICountryRepository
    {
        private readonly SplurgeStopDbContext _context;

        public CountryRepository(SplurgeStopDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsAsync(CountryId id)
        {
            return await _context.Countries.FindAsync(id) != null;
        }

        public async Task<Country> LoadCountryAsync(CountryId id)
        {
            return await _context.Countries.FindAsync(id);
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _context.Countries
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<CountryDto>> GetAllCountryDtoAsync()
        {
            return await _context.Countries
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
            return await _context.Countries
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCountryAsync(Country country)
        {
            await _context.Countries.AddAsync(country);
        }

        public async Task RemoveCountryAsync(CountryId id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country != null)
                _context.Countries.Remove(country);
        }
    }
}
