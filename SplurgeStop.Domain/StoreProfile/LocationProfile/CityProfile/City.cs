using System;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile.CityProfile
{
    public class City
    {
        private City() { }

        public static City Create(CityId id, string name)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id), "City without unique identifier cannot be created.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "City without name cannot be created.");

            var city = new City();

            city.Apply(new Events.CityCreated
            {
                Id = id,
                Name = name
            });

            return city;
        }

        public CityId Id { get; private set; }
        public string Name { get; private set; }

        public void UpdateCityName(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name), "A valid name for a city must be provided.");

            if (name.Length < 1 || name.Length > 128)
                throw new ArgumentException("Name length should be 128 characters or less, but not empty.", nameof(name));

            Name = name;
        }

        private void Apply(object @event)
        {
            When(@event);
        }

        private void When(object @event)
        {
            switch (@event)
            {
                case Events.CityCreated e:
                    Id = new CityId(e.Id);
                    Name = e.Name;
                    break;
                case Events.CityNameChanged e:
                    Name = e.Name;
                    break;
                case Events.CityDeleted e:
                    Id = new CityId(e.Id);
                    Name = e.Name;
                    break;
            }
        }

        internal bool EnsureValidState()
        {
            return Id.Value != default
                && !string.IsNullOrWhiteSpace(Name);
        }
    }
}