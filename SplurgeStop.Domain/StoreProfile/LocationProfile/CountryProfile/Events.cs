using System;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile
{
    public static class Events
    {
        public class CountryCreated
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class CountryNameChanged
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class CountryDeleted
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
