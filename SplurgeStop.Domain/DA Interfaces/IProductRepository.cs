using System;
using System.Threading.Tasks;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IProductRepository : IRepository<Product, ProductDto, ProductId>
    {
        // Queries
        Task<Product> GetProductFullAsync(ProductId id);
        Task<Product> LoadFullProductTrackedAsync(ProductId id);

        // Commands
        Task<Brand> GetBrandAsync(BrandId brandId);
        Task ChangeBrand(Product prod, BrandId brandId);
        Task ChangeProductType(Product prod, ProductTypeId productTypeId);
        Task ChangeSize(Product prod, SizeId sizeId);
        Task<ProductType> GetProductTypeAsync(ProductTypeId productTypeId);
        Task<Size> GetSizeAsync(SizeId sizeId);
    }
}
