using System.Collections.Generic;
using System.Threading.Tasks;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile
{
    public interface ICityService
    {
        Task Handle(object command);

        Task<City> GetCityAsync(CityId id);
        Task<IEnumerable<CityDto>> GetAllCityDtoAsync();
    }
}
