using System;

namespace SplurgeStop.Domain.ProductProfile.BrandProfile
{
    public static class Events
    {
        public class BrandCreated
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class BrandNameChanged
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class BrandDeleted
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
