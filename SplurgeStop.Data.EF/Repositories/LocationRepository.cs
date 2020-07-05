using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.CountryProfile;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.StoreProfile.LocationProfile;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class LocationRepository : ILocationRepository
    {
        private readonly SplurgeStopDbContext context;

        public LocationRepository(SplurgeStopDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsAsync(LocationId id)
        {
            return await context.Locations.FindAsync(id) != null;
        }

        public async Task<Location> LoadLocationAsync(LocationId id)
        {
            return await context.Locations
                .Include(c => c.City)
                .Include(c => c.Country)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await context.Locations
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<LocationDto>> GetAllLocationDtoAsync()
        {
            return await context.Locations
                    .Select(r => new LocationDto
                    {
                        Id = r.Id,
                        CityName = r.City.Name,
                        CountryName = r.Country.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<Location> GetLocationAsync(LocationId id)
        {
            return await context.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddLocationAsync(Location location)
        {
            await context.Locations.AddAsync(location);
        }

        public async Task RemoveLocationAsync(LocationId id)
        {
            var location = await context.Locations.FindAsync(id);

            if (location != null)
                context.Locations.Remove(location);
        }

        public async Task<City> GetCityAsync(CityId id)
        {
            return await context.Cities.FindAsync(id);
        }

        public async Task<Country> GetCountryAsync(CountryId id)
        {
            return await context.Countries.FindAsync(id);
        }

        public async Task ChangeCountry(Location location, CountryId countryId)
        {
            var modLocation = await context.Locations.FindAsync(location.Id);
            modLocation.UpdateCountry(await GetCountryAsync(countryId));
            await context.SaveChangesAsync();
        }

        public async Task ChangeCity(Location location, CityId cityId)
        {
            var modLocation = await context.Locations.FindAsync(location.Id);
            modLocation.UpdateCity(await GetCityAsync(cityId));
            await context.SaveChangesAsync();
        }
    }
}
