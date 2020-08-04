using System;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;

namespace SplurgeStop.Domain.ProductProfile
{
    public static class Events
    {
        public class ProductCreated
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public Brand Brand { get; set; }
            public ProductType ProductType { get; set; }
            public Size Size { get; set; }
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

        public class ProductTypeChanged
        {
            public Guid Id { get; set; }
            public ProductType ProductType { get; set; }
        }

        public class SizeChanged
        {
            public Guid Id { get; set; }
            public Size Size { get; set; }
        }

        public class ProductDeleted
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
