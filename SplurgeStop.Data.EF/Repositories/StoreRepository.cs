using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.Domain.StoreProfile.DTO;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class StoreRepository : IStoreRepository
    {
        private readonly SplurgeStopDbContext context;

        public StoreRepository(SplurgeStopDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddStoreAsync(Store store)
        {
            await context.Stores.AddAsync(store);
        }

        public async Task<bool> ExistsAsync(StoreId id)
        {
            return await context.Stores.FindAsync(id) != null;
        }

        public async Task<Store> GetStoreFullAsync(StoreId id)
        {
            return await context.Stores
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await context.Stores
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<Store> LoadStoreAsync(StoreId id)
        {
            return await context.Stores.FindAsync(id);
        }

        public async Task<Store> LoadFullStoreAsync(StoreId id)
        {
            return await context.Stores.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<StoreStripped>> GetAllStoresStrippedAsync()
        {
            return await context.Stores
                    .Select(r => new StoreStripped
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }
    }
}
