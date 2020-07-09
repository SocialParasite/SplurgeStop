using System;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using SplurgeStop.Domain.ProductProfile.TypeProfile;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class ProductTypeRepository : IRepository<ProductType, ProductTypeDto, ProductTypeId>
    {
        private readonly SplurgeStopDbContext _context;

        public ProductTypeRepository(SplurgeStopDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ProductTypeDto>> GetAllDtoAsync()
        {
            return await _context.ProductTypes
                    .Select(r => new ProductTypeDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }
        public async Task<ProductType> LoadAsync(ProductTypeId id)
        {
            return await _context.ProductTypes.FindAsync(id);
        }

        public async Task<ProductType> GetAsync(ProductTypeId id)
        {
            return await _context.ProductTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(ProductTypeId id)
        {
            return await _context.ProductTypes.FindAsync(id) != null;
        }

        public async Task AddAsync(ProductType productType)
        {
            await _context.ProductTypes.AddAsync(productType);
        }

        public async Task RemoveAsync(ProductTypeId id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);

            if (productType != null)
                _context.ProductTypes.Remove(productType);
        }
    }
}
