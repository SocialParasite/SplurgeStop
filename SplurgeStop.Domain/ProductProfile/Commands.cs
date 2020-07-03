using System;

namespace SplurgeStop.Domain.ProductProfile
{
    public static class Commands
    {
        public class Create
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
            public Guid? BrandId { get; set; }

        }
        public class UpdateProduct
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public Guid? BrandId { get; set; }
        }

        public class ChangeBrand
        {
            public Guid Id { get; set; }
            public Guid BrandId { get; set; }
        }

        public class ChangeProductType
        {
            public Guid Id { get; set; }
            public Guid ProductTypeId { get; set; }
        }

        public class ChangeSize
        {
            public Guid Id { get; set; }
            public Guid SizeId { get; set; }
        }
        public class DeleteProduct
        {
            public Guid Id { get; set; }
        }
    }
}
