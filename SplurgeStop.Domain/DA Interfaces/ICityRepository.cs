using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.StoreProfile.Location.CityProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface ICityRepository
    {
        // Queries
        Task<City> LoadCityAsync(CityId id);
        Task<IEnumerable<City>> GetAllCitiesAsync();
        Task<IEnumerable<CityDto>> GetAllCityDtoAsync();
        Task<City> GetCityAsync(CityId id);
        Task<bool> ExistsAsync(CityId id);

        // Commands
        Task AddCityAsync(City city);
        Task RemoveCityAsync(CityId id);
    }
}
