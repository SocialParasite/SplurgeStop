using System;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile
{
    public static class Commands
    {
        public class Create
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
        }
        public class SetCityName
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class DeleteCity
        {
            public Guid Id { get; set; }
        }
    }
}
