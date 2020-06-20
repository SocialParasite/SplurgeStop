using System;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.CountryProfile;

namespace SplurgeStop.Domain.LocationProfile
{
    public class Location
    {
        public static Location Create(LocationId id, City city, Country country)
        {
            var location = new Location();

            location.Apply(new Events.LocationCreated
            {
                Id = id,
                City = city,
                Country = country
            });

            return location;
        }

        public LocationId Id { get; private set; }
        public City City { get; private set; }
        public Country Country { get; private set; }

        public void UpdateCountry(Country country)
        {
            Country = country ?? throw new ArgumentNullException("A country must be provided.");
        }

        public void UpdateCity(City city)
        {
            City = city ?? throw new ArgumentNullException("A city must be provided.");
        }

        private void Apply(object @event)
        {
            When(@event);
        }

        private void When(object @event)
        {
            switch (@event)
            {
                case Events.LocationCreated e:
                    Id = new LocationId(e.Id);
                    City = e.City;
                    Country = e.Country;
                    break;
                case Events.CountryChanged e:
                    Country = Country;
                    break;
                case Events.CityChanged e:
                    City = e.City;
                    break;
                case Events.LocationDeleted e:
                    Id = new LocationId(e.Id);
                    break;
            }
        }

        internal bool EnsureValidState()
        {
            return Id.Value != default
                && City != null
                && Country != null;
        }
    }
}
