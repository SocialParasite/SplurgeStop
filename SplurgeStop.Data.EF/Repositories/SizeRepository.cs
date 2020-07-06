using System;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class SizeRepository : ISizeRepository
    {
        private readonly SplurgeStopDbContext _context;

        public SizeRepository(SplurgeStopDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsAsync(SizeId id)
        {
            return await _context.Size.FindAsync(id) != null;
        }

        public async Task<Size> LoadSizeAsync(SizeId id)
        {
            return await _context.Size.FindAsync(id);
        }

        public async Task<IEnumerable<Size>> GetAllSizesAsync()
        {
            return await _context.Size
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<SizeDto>> GetAllSizeDtoAsync()
        {
            return await _context.Size
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
            return await _context.Size
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddSizeAsync(Size size)
        {
            await _context.Size.AddAsync(size);
        }

        public async Task RemoveSizeAsync(SizeId id)
        {
            var size = await _context.Size.FindAsync(id);

            if (size != null)
                _context.Size.Remove(size);
        }
    }
}
