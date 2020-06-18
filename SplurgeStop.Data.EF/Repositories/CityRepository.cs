﻿using System;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.CityProfile;
using Microsoft.EntityFrameworkCore;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class CityRepository : ICityRepository
    {
        private readonly SplurgeStopDbContext context;

        public CityRepository(SplurgeStopDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsAsync(CityId id)
        {
            return await context.Cities.FindAsync(id) != null;
        }

        public async Task<City> LoadCityAsync(CityId id)
        {
            return await context.Cities.FindAsync(id);
        }

        public async Task<City> GetCityAsync(CityId id)
        {
            return await context.Cities
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCityAsync(City city)
        {
            await context.Cities.AddAsync(city);
        }

        public async Task RemoveCityAsync(CityId id)
        {
            var city = await context.Cities.FindAsync(id);

            if (city != null)
                context.Cities.Remove(city);
        }
    }
}
