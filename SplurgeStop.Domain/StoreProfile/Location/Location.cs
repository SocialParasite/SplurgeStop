using System;
using System.Collections.Generic;
using System.Text;

namespace SplurgeStop.Domain.StoreProfile
{
    public class Location
    {
        public LocationId Id { get; }
        public City City { get; set; }
        public Country Country { get; set; }
    }
}
