using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.ProductProfile.DTO;
using SplurgeStop.Domain.ProductProfile.TypeProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IProductTypeRepository
    {
        // Queries
        Task<ProductType> LoadProductTypeAsync(ProductTypeId id);
        Task<IEnumerable<ProductType>> GetAllProductTypesAsync();
        Task<IEnumerable<ProductTypeDto>> GetAllProductTypeDtoAsync();
        Task<ProductType> GetProductTypeAsync(ProductTypeId id);
        Task<bool> ExistsAsync(ProductTypeId id);

        // Commands
        Task AddProductTypeAsync(ProductType productType);
        Task RemoveProductTypeAsync(ProductTypeId id);
    }
}
