using System;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile
{
    public class Country
    {
        public static Country Create(CountryId id, string name)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id), "Country without unique identifier cannot be created.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Country without name cannot be created.");

            var country = new Country();

            country.Apply(new Events.CountryCreated
            {
                Id = id,
                Name = name
            });

            return country;
        }

        public CountryId Id { get; private set; }
        public string Name { get; set; }

        public void UpdateCountryName(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name), "A valid name for a country must be provided.");

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
                case Events.CountryCreated e:
                    Id = new CountryId(e.Id);
                    Name = e.Name;
                    break;
                case Events.CountryNameChanged e:
                    Name = e.Name;
                    break;
                case Events.CountryDeleted e:
                    Id = new CountryId(e.Id);
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