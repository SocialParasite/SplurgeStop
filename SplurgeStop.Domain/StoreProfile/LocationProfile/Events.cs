using System;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.CountryProfile;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile
{
    public static class Events
    {
        public class LocationCreated
        {
            public Guid Id { get; set; }
            public City City { get; set; }
            public Country Country { get; set; }
        }

        public class CityChanged
        {
            public Guid Id { get; set; }
            public City City { get; set; }

        }

        public class CountryChanged
        {
            public Guid Id { get; set; }
            public Country Country { get; set; }

        }

        public class LocationDeleted
        {
            public Guid Id { get; set; }
        }
    }
}
