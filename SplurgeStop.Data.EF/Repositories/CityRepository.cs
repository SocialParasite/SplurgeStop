using System;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.CityProfile;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class CityRepository : ICityRepository
    {
        private readonly SplurgeStopDbContext _context;

        public CityRepository(SplurgeStopDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsAsync(CityId id)
        {
            return await _context.Cities.FindAsync(id) != null;
        }

        public async Task<City> LoadCityAsync(CityId id)
        {
            return await _context.Cities.FindAsync(id);
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            return await _context.Cities
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<CityDto>> GetAllCityDtoAsync()
        {
            return await _context.Cities
                    .Select(r => new CityDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<City> GetCityAsync(CityId id)
        {
            return await _context.Cities
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCityAsync(City city)
        {
            await _context.Cities.AddAsync(city);
        }

        public async Task RemoveCityAsync(CityId id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city != null)
                _context.Cities.Remove(city);
        }
    }
}
