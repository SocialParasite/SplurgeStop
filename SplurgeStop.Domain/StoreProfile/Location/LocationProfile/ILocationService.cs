using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.LocationProfile.DTO;

namespace SplurgeStop.Domain.LocationProfile
{
    public interface ILocationService
    {
        Task Handle(object command);

        Task<Location> GetLocationAsync(LocationId id);
        Task<IEnumerable<LocationDto>> GetAllLocationDtoAsync();
    }
}
