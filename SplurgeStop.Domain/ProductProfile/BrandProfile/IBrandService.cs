using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.ProductProfile.DTO;

namespace SplurgeStop.Domain.ProductProfile.BrandProfile
{
    public interface IBrandService
    {
        Task Handle(object command);

        Task<Brand> GetBrandAsync(BrandId id);
        Task<IEnumerable<BrandDto>> GetAllBrandDtoAsync();
    }
}
