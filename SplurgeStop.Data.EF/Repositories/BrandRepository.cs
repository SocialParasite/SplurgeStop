﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.DTO;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class BrandRepository : IBrandRepository
    {
        private readonly SplurgeStopDbContext context;

        public BrandRepository(SplurgeStopDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ExistsAsync(BrandId id)
        {
            return await context.Brands.FindAsync(id) != null;
        }

        public async Task<Brand> LoadBrandAsync(BrandId id)
        {
            return await context.Brands.FindAsync(id);
        }

        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await context.Brands
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<IEnumerable<BrandDto>> GetAllBrandDtoAsync()
        {
            return await context.Brands
                    .Select(r => new BrandDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<Brand> GetBrandAsync(BrandId id)
        {
            return await context.Brands
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddBrandAsync(Brand brand)
        {
            await context.Brands.AddAsync(brand);
        }

        public async Task RemoveBrandAsync(BrandId id)
        {
            var brand = await context.Brands.FindAsync(id);

            if (brand != null)
                context.Brands.Remove(brand);
        }
    }
}