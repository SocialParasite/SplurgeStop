using System;

namespace SplurgeStop.Domain.ProductProfile.BrandProfile
{
    public static class Commands
    {
        public class Create
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
        }
        public class SetBrandName
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class DeleteBrand
        {
            public Guid Id { get; set; }
        }
    }
}
