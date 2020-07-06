using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface ICountryRepository
    {
        // Queries
        Task<Country> LoadCountryAsync(CountryId id);
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<IEnumerable<CountryDto>> GetAllCountryDtoAsync();
        Task<Country> GetCountryAsync(CountryId id);
        Task<bool> ExistsAsync(CountryId id);

        // Commands
        Task AddCountryAsync(Country country);
        Task RemoveCountryAsync(CountryId id);
    }
}
