using System;

namespace SplurgeStop.Domain.StoreProfile
{
    public static class Events
    {
        public class StoreCreated
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public LocationProfile.Location Location { get; set; }
        }

        public class StoreChanged
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public LocationProfile.Location Location { get; set; }

        }

        public class StoreDeleted
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
