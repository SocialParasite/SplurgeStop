using System.Collections.Generic;
using System.Threading.Tasks;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile
{
    public interface ICountryService
    {
        Task Handle(object command);

        Task<Country> GetCountryAsync(CountryId id);
        Task<IEnumerable<CountryDto>> GetAllCountryDtoAsync();
    }
}
