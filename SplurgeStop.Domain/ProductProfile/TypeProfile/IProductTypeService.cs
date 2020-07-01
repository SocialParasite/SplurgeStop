using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.ProductProfile.DTO;

namespace SplurgeStop.Domain.ProductProfile.TypeProfile
{
    public interface IProductTypeService
    {
        Task Handle(object command);

        Task<ProductType> GetProductTypeAsync(ProductTypeId id);
        Task<IEnumerable<ProductTypeDto>> GetAllProductTypeDtoAsync();
    }
}
