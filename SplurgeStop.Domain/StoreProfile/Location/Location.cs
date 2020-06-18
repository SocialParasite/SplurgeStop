using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.CountryProfile;

namespace SplurgeStop.Domain.StoreProfile
{
    public class Location
    {
        public LocationId Id { get; }
        public City City { get; set; }
        public Country Country { get; set; }
    }
}
