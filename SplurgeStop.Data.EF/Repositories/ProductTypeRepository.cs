using System;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using SplurgeStop.Domain.ProductProfile.DTO;
using SplurgeStop.Domain.ProductProfile.TypeProfile;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class ProductTypeRepository : IProductTypeRepository
    {
        private readonly SplurgeStopDbContext context;

        public ProductTypeRepository(SplurgeStopDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsAsync(ProductTypeId id)
        {
            return await context.ProductTypes.FindAsync(id) != null;
        }

        public async Task<ProductType> LoadProductTypeAsync(ProductTypeId id)
        {
            return await context.ProductTypes.FindAsync(id);
        }

        public async Task<IEnumerable<ProductType>> GetAllProductTypesAsync()
        {
            return await context.ProductTypes
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<ProductTypeDto>> GetAllProductTypeDtoAsync()
        {
            return await context.ProductTypes
                    .Select(r => new ProductTypeDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<ProductType> GetProductTypeAsync(ProductTypeId id)
        {
            return await context.ProductTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddProductTypeAsync(ProductType productType)
        {
            await context.ProductTypes.AddAsync(productType);
        }

        public async Task RemoveProductTypeAsync(ProductTypeId id)
        {
            var productType = await context.ProductTypes.FindAsync(id);

            if (productType != null)
                context.ProductTypes.Remove(productType);
        }
    }
}
