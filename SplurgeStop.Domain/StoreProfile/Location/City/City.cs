using System;

namespace SplurgeStop.Domain.CityProfile
{
    public class City
    {
        public static City Create(CityId id, string name)
        {
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

        internal void UpdateCityName(string name)
        {
            Name = name ?? throw new ArgumentNullException("A valid name for city must be provided.");
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