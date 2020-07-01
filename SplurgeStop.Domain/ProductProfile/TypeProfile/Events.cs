using System;

namespace SplurgeStop.Domain.ProductProfile.TypeProfile
{
    public static class Events
    {
        public class ProductTypeCreated
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class ProductTypeNameChanged
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class ProductTypeDeleted
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
