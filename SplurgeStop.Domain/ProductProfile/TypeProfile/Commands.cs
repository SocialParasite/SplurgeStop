using System;

namespace SplurgeStop.Domain.ProductProfile.TypeProfile
{
    public static class Commands
    {
        public class Create
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
        }
        public class SetProductTypeName
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class DeleteProductType
        {
            public Guid Id { get; set; }
        }
    }
}
