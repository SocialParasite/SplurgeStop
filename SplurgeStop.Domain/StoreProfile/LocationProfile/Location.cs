﻿using System;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile;
using SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile
{
    public class Location
    {
        private Location() { }

        public static Location Create(LocationId id, City city, Country country)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id), "Location without unique identifier cannot be created.");

            if (city is null)
                throw new ArgumentNullException(nameof(city), "Location without a city cannot be created.");

            if (country is null)
                throw new ArgumentNullException(nameof(country), "Location without a country cannot be created.");

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
            Country = country ?? throw new ArgumentNullException(nameof(country), "A country must be provided.");
        }

        public void UpdateCity(City city)
        {
            City = city ?? throw new ArgumentNullException(nameof(city), "A city must be provided.");
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
