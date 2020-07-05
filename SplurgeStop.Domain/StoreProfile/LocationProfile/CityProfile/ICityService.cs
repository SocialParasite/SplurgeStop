using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.StoreProfile.Location.CityProfile;

namespace SplurgeStop.Domain.CityProfile
{
    public interface ICityService
    {
        Task Handle(object command);

        Task<City> GetCityAsync(CityId id);
        Task<IEnumerable<CityDto>> GetAllCityDtoAsync();
    }
}
