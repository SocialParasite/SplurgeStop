using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.ProductProfile.BrandProfile;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class BrandRepository : IRepository<Brand, BrandDto, BrandId>
    {
        private readonly SplurgeStopDbContext _context;

        public BrandRepository(SplurgeStopDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsAsync(BrandId id)
        {
            return await _context.Brands.FindAsync(id) != null;
        }

        public async Task<Brand> LoadAsync(BrandId id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task<IEnumerable<Brand>> GetAllAsync()
        {
            return await _context.Brands
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<BrandDto>> GetAllDtoAsync()
        {
            return await _context.Brands
                    .Select(r => new BrandDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<Brand> GetAsync(BrandId id)
        {
            return await _context.Brands
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
        }

        public async Task RemoveAsync(BrandId id)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand != null)
                _context.Brands.Remove(brand);
        }
    }
}
