using System;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.CountryProfile;

namespace SplurgeStop.Domain.LocationProfile
{
    public static class Commands
    {
        public class Create
        {
            public Guid? Id { get; set; }
            public Guid CityId { get; set; }
            public Guid CountryId { get; set; }
        }

        public class ChangeCity
        {
            public Guid Id { get; set; }
            public City City { get; set; }

        }

        public class ChangeCountry
        {
            public Guid Id { get; set; }
            public Country Country { get; set; }

        }

        public class DeleteLocation
        {
            public Guid Id { get; set; }
        }
    }
}
