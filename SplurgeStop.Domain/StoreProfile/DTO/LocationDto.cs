using System;

namespace SplurgeStop.Domain.LocationProfile.DTO
{
    public sealed class LocationDto
    {
        public Guid Id { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
    }
}
