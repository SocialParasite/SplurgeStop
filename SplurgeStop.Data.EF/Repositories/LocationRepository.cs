using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class LocationRepository : ILocationRepository
    {
        private readonly SplurgeStopDbContext _context;

        public LocationRepository(SplurgeStopDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<LocationDto>> GetAllDtoAsync()
        {
            return await _context.Locations
                    .Select(r => new LocationDto
                    {
                        Id = r.Id,
                        CityName = r.City.Name,
                        CountryName = r.Country.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }
        public async Task<Location> LoadAsync(LocationId id)
        {
            return await _context.Locations
                .Include(c => c.City)
                .Include(c => c.Country)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Location> GetAsync(LocationId id)
        {
            return await _context.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(LocationId id)
        {
            return await _context.Locations.FindAsync(id) != null;
        }

        public async Task AddAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
        }

        public async Task RemoveAsync(LocationId id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location != null)
                _context.Locations.Remove(location);
        }

        public async Task<City> GetCityAsync(CityId id)
        {
            return await _context.Cities.FindAsync(id);
        }

        public async Task<Country> GetCountryAsync(CountryId id)
        {
            return await _context.Countries.FindAsync(id);
        }

        public async Task ChangeCity(Location location, CityId cityId)
        {
            var modLocation = await _context.Locations.FindAsync(location.Id);
            modLocation.UpdateCity(await GetCityAsync(cityId));
            await _context.SaveChangesAsync();
        }

        public async Task ChangeCountry(Location location, CountryId countryId)
        {
            var modLocation = await _context.Locations.FindAsync(location.Id);
            modLocation.UpdateCountry(await GetCountryAsync(countryId));
            await _context.SaveChangesAsync();
        }

    }
}
