using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IProductRepository
    {
        // Queries
        Task<Product> LoadProductAsync(ProductId id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<ProductDto>> GetAllProductDtoAsync();
        Task<Product> GetProductAsync(ProductId id);
        Task<Product> GetProductFullAsync(ProductId id);
        Task<Product> LoadFullProductAsync(ProductId id);
        Task<bool> ExistsAsync(ProductId id);

        // Commands
        Task AddProductAsync(Product product);
        Task RemoveProductAsync(ProductId id);
        Task<Brand> GetBrandAsync(BrandId brandId);
        Task ChangeBrand(Product prod, BrandId brandId);
        Task ChangeProductType(Product prod, ProductTypeId productTypeId);
        Task ChangeSize(Product prod, SizeId sizeId);
    }
}
