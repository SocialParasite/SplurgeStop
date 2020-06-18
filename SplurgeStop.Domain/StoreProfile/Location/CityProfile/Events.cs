using System;

namespace SplurgeStop.Domain.CityProfile
{
    public static class Events
    {
        public class CityCreated
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class CityNameChanged
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class CityDeleted
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
