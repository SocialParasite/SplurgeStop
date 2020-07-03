using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.ProductProfile.DTO;

namespace SplurgeStop.Domain.ProductProfile.SizeProfile
{
    public interface ISizeService
    {
        Task Handle(object command);

        Task<Size> GetSizeAsync(SizeId id);
        Task<IEnumerable<SizeDto>> GetAllSizeDtoAsync();
    }
}
