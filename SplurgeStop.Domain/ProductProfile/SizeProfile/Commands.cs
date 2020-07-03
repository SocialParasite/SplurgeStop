using System;

namespace SplurgeStop.Domain.ProductProfile.SizeProfile
{
    public static class Commands
    {
        public class Create
        {
            public Guid? Id { get; set; }
            public string Amount { get; set; }
        }
        public class SetSizeAmount
        {
            public Guid Id { get; set; }
            public string Amount { get; set; }
        }

        public class DeleteSize
        {
            public Guid Id { get; set; }
        }
    }
}
