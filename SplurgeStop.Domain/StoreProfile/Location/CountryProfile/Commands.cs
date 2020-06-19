using System;

namespace SplurgeStop.Domain.CountryProfile
{
    public static class Commands
    {
        public class Create
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
        }
        public class SetCountryName
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class DeleteCountry
        {
            public Guid Id { get; set; }
        }
    }
}
