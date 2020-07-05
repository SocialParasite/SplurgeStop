using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.CountryProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface ILocationRepository
    {
        // Queries
        Task<Location> LoadLocationAsync(LocationId id);
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<IEnumerable<LocationDto>> GetAllLocationDtoAsync();
        Task<Location> GetLocationAsync(LocationId id);
        Task<bool> ExistsAsync(LocationId id);

        Task<Country> GetCountryAsync(CountryId id);
        Task<City> GetCityAsync(CityId id);

        // Commands
        Task AddLocationAsync(Location location);
        Task RemoveLocationAsync(LocationId id);
        Task ChangeCountry(Location location, CountryId countryId);
        Task ChangeCity(Location location, CityId cityId);
    }
}
