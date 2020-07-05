using System;
using SplurgeStop.Domain.LocationProfile;

namespace SplurgeStop.Domain.StoreProfile
{
    public class Store
    {
        public static Store Create(StoreId id, string name, Location location = null)
        {
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

        internal void UpdateStoreName(string name)
        {
            Name = name ?? throw new ArgumentNullException("A valid name for store must be provided.");
        }

        public void UpdateLocation(Location location)
        {
            Location = location ?? throw new ArgumentNullException("A location must be provided.");
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
