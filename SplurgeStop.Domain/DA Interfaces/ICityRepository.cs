using System.Threading.Tasks;
using SplurgeStop.Domain.CityProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface ICityRepository
    {
        Task<City> LoadCityAsync(CityId id);
        Task<City> GetCityAsync(CityId id);
        Task<bool> ExistsAsync(CityId id);
        Task AddCityAsync(City city);
        Task RemoveCityAsync(CityId id);
    }
}
