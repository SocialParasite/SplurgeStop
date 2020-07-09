using System.Threading.Tasks;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface ILocationRepository : IRepository<Location, LocationDto, LocationId>

    {
        // Queries
        Task<City> GetCityAsync(CityId id);
        Task<Country> GetCountryAsync(CountryId id);

        // Commands
        Task ChangeCity(Location location, CityId cityId);
        Task ChangeCountry(Location location, CountryId countryId);
    }
}
