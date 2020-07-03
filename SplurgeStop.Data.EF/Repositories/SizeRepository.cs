using System;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SplurgeStop.Domain.ProductProfile.DTO;
using System.Linq;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class SizeRepository : ISizeRepository
    {
        private readonly SplurgeStopDbContext context;

        public SizeRepository(SplurgeStopDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsAsync(SizeId id)
        {
            return await context.Size.FindAsync(id) != null;
        }

        public async Task<Size> LoadSizeAsync(SizeId id)
        {
            return await context.Size.FindAsync(id);
        }

        public async Task<IEnumerable<Size>> GetAllSizesAsync()
        {
            return await context.Size
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<SizeDto>> GetAllSizeDtoAsync()
        {
            return await context.Size
                .Select(r => new SizeDto
                {
                    Id = r.Id,
                    Amount = r.Amount
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Size> GetSizeAsync(SizeId id)
        {
            return await context.Size
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddSizeAsync(Size size)
        {
            await context.Size.AddAsync(size);
        }

        public async Task RemoveSizeAsync(SizeId id)
        {
            var size = await context.Size.FindAsync(id);

            if (size != null)
                context.Size.Remove(size);
        }
    }
}
