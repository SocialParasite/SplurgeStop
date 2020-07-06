using System.Threading.Tasks;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface ILocationRepository : IRepository<Location, LocationDto, LocationId>

    {
        // Queries
        Task<Country> GetCountryAsync(CountryId id);
        Task<City> GetCityAsync(CityId id);

        // Commands
        Task ChangeCountry(Location location, CountryId countryId);
        Task ChangeCity(Location location, CityId cityId);
    }
}
