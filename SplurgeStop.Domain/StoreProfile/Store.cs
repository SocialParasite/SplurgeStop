using System;
using SplurgeStop.Domain.StoreProfile.LocationProfile;

namespace SplurgeStop.Domain.StoreProfile
{
    public class Store
    {
        private Store() { }

        public static Store Create(StoreId id, string name, Location location = null)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id), "Store without unique identifier cannot be created.");

            if (name is null)
                throw new ArgumentNullException(nameof(name), "Store without name cannot be created.");

            var store = new Store();

            store.Apply(new Events.StoreCreated
            {
                Id = id,
                Name = name,
                Location = location
            });

            return store;
        }

        public StoreId Id { get; private set; }
        public string Name { get; private set; }
        public Location Location { get; private set; }

        // chain, Kesko
        // store type, Citymarket

        public void UpdateStoreName(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name), "A valid name for store must be provided.");

            if (name.Length < 1 || name.Length > 128)
                throw new ArgumentException("Name length should be 128 characters or less, but not empty.", nameof(name));

            Name = name;
        }

        public void UpdateLocation(Location location)
        {
            Location = location ?? throw new ArgumentNullException(nameof(location), "A location must be provided.");
        }

        private void Apply(object @event)
        {
            When(@event);
        }

        private void When(object @event)
        {
            switch (@event)
            {
                case Events.StoreCreated e:
                    Id = new StoreId(e.Id);
                    Name = e.Name;
                    Location = e.Location;
                    break;
                case Events.StoreChanged e:
                    Name = e.Name;
                    Location = e.Location;
                    break;
                case Events.StoreDeleted e:
                    Id = new StoreId(e.Id);
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
