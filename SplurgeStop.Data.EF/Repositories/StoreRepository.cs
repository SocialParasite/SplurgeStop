using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class StoreRepository : IStoreRepository
    {
        private readonly SplurgeStopDbContext _context;

        public StoreRepository(SplurgeStopDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<StoreStripped>> GetAllDtoAsync()
        {
            return await _context.Stores
                    .Select(r => new StoreStripped
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<Store> LoadAsync(StoreId id)
        {
            return await _context.Stores.FindAsync(id);
        }

        public async Task<Store> GetAsync(StoreId id)
        {
            return await _context.Stores
                .AsNoTracking()
                .Include(s => s.Location)
                .ThenInclude(l => l.City)
                .Include(s => s.Location)
                .ThenInclude(l => l.Country)
                .FirstOrDefaultAsync(s => s.Id == id);
        }


        public async Task<bool> ExistsAsync(StoreId id)
        {
            return await _context.Stores.FindAsync(id) != null;
        }

        public async Task AddAsync(Store store)
        {
            await _context.Stores.AddAsync(store);
        }

        public async Task RemoveAsync(StoreId id)
        {
            var store = await _context.Stores.FindAsync(id);

            if (store != null)
                _context.Stores.Remove(store);
        }

        public async Task<Store> GetTrackedAsync(StoreId id)
        {
            return await _context.Stores
                .Include(s => s.Location)
                .ThenInclude(l => l.City)
                .Include(s => s.Location)
                .ThenInclude(l => l.Country)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Location> GetLocationAsync(LocationId id)
        {
            return await _context.Locations.FindAsync(id);
        }

        public async Task ChangeLocation(Store store, LocationId locationId)
        {
            var modStore = await _context.Stores.FindAsync(store.Id);
            modStore.UpdateLocation(await GetLocationAsync(locationId));
            await _context.SaveChangesAsync();
        }

    }
}
