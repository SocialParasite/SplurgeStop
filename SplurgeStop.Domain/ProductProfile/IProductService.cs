using System.Collections.Generic;
using System.Threading.Tasks;

namespace SplurgeStop.Domain.ProductProfile
{
    public interface IProductService
    {
        Task Handle(object command);

        Task<Product> GetProductAsync(ProductId id);
        Task<IEnumerable<ProductDto>> GetAllProductDtoAsync();
    }
}
