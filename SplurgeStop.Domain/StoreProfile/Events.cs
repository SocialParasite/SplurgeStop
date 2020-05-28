using System;

namespace SplurgeStop.Domain.StoreProfile
{
    public static class Events
    {
        public class StoreCreated
        {
            public Guid Id { get; set; }
        }

        public class StoreNameChanged
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
