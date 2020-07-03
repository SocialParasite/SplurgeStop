using System;
using SplurgeStop.Domain.ProductProfile.BrandProfile;

namespace SplurgeStop.Domain.ProductProfile
{
    public static class Events
    {
        public class ProductCreated
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public Brand Brand { get; set; }
        }

        public class ProductNameChanged
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class BrandChanged
        {
            public Guid Id { get; set; }
            public Brand Brand { get; set; }
        }

        public class ProductDeleted
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
