using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.CityProfile.DTO;

namespace SplurgeStop.Domain.CityProfile
{
    public interface ICityService
    {
        Task Handle(object command);

        Task<City> GetCityAsync(CityId id);
        Task<IEnumerable<CityDto>> GetAllCityDtoAsync();
    }
}
