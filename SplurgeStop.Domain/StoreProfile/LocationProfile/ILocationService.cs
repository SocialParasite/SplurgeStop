using System.Collections.Generic;
using System.Threading.Tasks;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile
{
    public interface ILocationService
    {
        Task Handle(object command);

        Task<Location> GetLocationAsync(LocationId id);
        Task<IEnumerable<LocationDto>> GetAllLocationDtoAsync();
    }
}
