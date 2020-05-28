using System;
using GuidHelpers;

namespace SplurgeStop.Domain.StoreProfile
{
    public class Store
    {
        public Store()
        {
            Apply(new Events.StoreCreated
            {
                Id = new StoreId(SequentialGuid.NewSequentialGuid())
            });
        }

        public StoreId Id { get; private set; }
        public string Name { get; set; }
        // store name, K-Citymarket Länsikeskus
        // chain, Kesko
        // store type, Citymarket
        // store location? Länsikeskus / Turku??? Finland!!

        internal void UpdateStoreName(string name)
        {
            Name = name ?? throw new ArgumentNullException("A valid name for store must be provided.");
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
                    Name = string.Empty;
                    break;
                case Events.StoreNameChanged e:
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
