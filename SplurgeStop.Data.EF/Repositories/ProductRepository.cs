using System;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using SplurgeStop.Domain.ProductProfile.DTO;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly SplurgeStopDbContext context;

        public ProductRepository(SplurgeStopDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsAsync(ProductId id)
        {
            return await context.Products.FindAsync(id) != null;
        }

        public async Task<Product> LoadProductAsync(ProductId id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await context.Products
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductDtoAsync()
        {
            return await context.Products
                    .Select(r => new ProductDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<Product> GetProductAsync(ProductId id)
        {
            return await context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Product> GetProductFullAsync(ProductId id)
        {
            return await context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductType)
                .Include(p => p.Size)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Product> LoadFullProductAsync(ProductId id)
        {
            return await context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductType)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddProductAsync(Product product)
        {
            await context.Products.AddAsync(product);
        }

        public async Task RemoveProductAsync(ProductId id)
        {
            var product = await context.Products.FindAsync(id);

            if (product != null)
                context.Products.Remove(product);
        }

        public async Task<Brand> GetBrandAsync(BrandId brandId)
        {
            return await context.Brands.FindAsync(brandId);
        }

        public async Task ChangeBrand(Product prod, BrandId brandId)
        {
            var product = await context.Products.FindAsync(prod.Id);
            product.UpdateBrand(await GetBrandAsync(brandId));
            await context.SaveChangesAsync();
        }

        public async Task ChangeProductType(Product prod, ProductTypeId productTypeId)
        {
            var product = await context.Products.FindAsync(prod.Id);
            product.UpdateProductType(await GetProductTypeAsync(productTypeId));
            await context.SaveChangesAsync();
        }

        private async Task<ProductType> GetProductTypeAsync(ProductTypeId productTypeId)
        {
            return await context.ProductTypes.FindAsync(productTypeId);
        }

        public async Task ChangeSize(Product prod, SizeId sizeId)
        {
            var product = await context.Products.FindAsync(prod.Id);
            product.UpdateSize(await GetSizeAsync(sizeId));
            await context.SaveChangesAsync();
        }

        private async Task<Size> GetSizeAsync(SizeId sizeId)
        {
            return await context.Size.FindAsync(sizeId);
        }
    }
}
