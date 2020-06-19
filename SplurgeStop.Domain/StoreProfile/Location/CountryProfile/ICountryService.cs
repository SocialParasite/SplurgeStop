using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.CountryProfile.DTO;

namespace SplurgeStop.Domain.CountryProfile
{
    public interface ICountryService
    {
        Task Handle(object command);

        Task<Country> GetCountryAsync(CountryId id);
        Task<IEnumerable<CountryDto>> GetAllCountryDtoAsync();
    }
}
