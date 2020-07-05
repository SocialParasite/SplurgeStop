using System;

namespace SplurgeStop.Domain.StoreProfile
{
    public static class Commands
    {
        public class Create
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
            public Guid? LocationId { get; set; }
        }

        public class UpdateStore
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public Guid LocationId { get; set; }
        }

        public class ChangeLocation
        {
            public Guid Id { get; set; }
            public LocationProfile.Location Location { get; set; }
        }

        public class DeleteStore
        {
            public Guid Id { get; set; }
        }
    }
}
