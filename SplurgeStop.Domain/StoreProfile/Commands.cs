using System;

namespace SplurgeStop.Domain.StoreProfile
{
    public static class Commands
    {
        public class Create
        {
            public Store Store { get; set; }
        }

        public class SetStoreName
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
