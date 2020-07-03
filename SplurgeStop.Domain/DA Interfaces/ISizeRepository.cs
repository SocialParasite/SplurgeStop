using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.DTO;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface ISizeRepository
    {
        // Queries
        Task<Size> LoadSizeAsync(SizeId id);
        Task<IEnumerable<Size>> GetAllSizesAsync();
        Task<IEnumerable<SizeDto>> GetAllSizeDtoAsync();
        Task<Size> GetSizeAsync(SizeId id);
        Task<bool> ExistsAsync(SizeId id);

        // Commands
        Task AddSizeAsync(Size size);
        Task RemoveSizeAsync(SizeId id);
    }
}
