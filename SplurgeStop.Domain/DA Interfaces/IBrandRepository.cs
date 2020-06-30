using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.DTO;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IBrandRepository
    {
        // Queries
        Task<Brand> LoadBrandAsync(BrandId id);
        Task<IEnumerable<Brand>> GetAllBrandsAsync();
        Task<IEnumerable<BrandDto>> GetAllBrandDtoAsync();
        Task<Brand> GetBrandAsync(BrandId id);
        Task<bool> ExistsAsync(BrandId id);

        // Commands
        Task AddBrandAsync(Brand brand);
        Task RemoveBrandAsync(BrandId id);
    }
}
