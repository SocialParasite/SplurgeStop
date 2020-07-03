using System;

namespace SplurgeStop.Domain.ProductProfile.SizeProfile
{
    public static class Events
    {
        public class SizeCreated
        {
            public Guid Id { get; set; }
            public string Amount { get; set; }
        }

        public class SizeAmountChanged
        {
            public Guid Id { get; set; }
            public string Amount { get; set; }
        }

        public class SizeDeleted
        {
            public Guid Id { get; set; }
            public string Amount { get; set; }
        }
    }
}
