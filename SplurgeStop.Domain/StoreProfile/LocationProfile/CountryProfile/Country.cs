using System;

namespace SplurgeStop.Domain.StoreProfile.LocationProfile.CountryProfile
{
    public class Country
    {
        public static Country Create(CountryId id, string name)
        {
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

        internal void UpdateCountryName(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "A valid name for country must be provided.");
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