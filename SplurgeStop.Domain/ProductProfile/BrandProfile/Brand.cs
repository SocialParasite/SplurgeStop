using System;

namespace SplurgeStop.Domain.ProductProfile.BrandProfile
{
    public class Brand
    {
        public static Brand Create(BrandId id, string name)
        {
            var brand = new Brand();

            brand.Apply(new Events.BrandCreated
            {
                Id = id,
                Name = name
            });

            return brand;
        }

        public BrandId Id { get; private set; }
        public string Name { get; private set; }

        public void UpdateBrandName(string name)
        {
            Name = name ?? throw new ArgumentNullException("A valid name for brand must be provided.");
        }

        private void Apply(object @event)
        {
            When(@event);
        }

        private void When(object @event)
        {
            switch (@event)
            {
                case Events.BrandCreated e:
                    Id = new BrandId(e.Id);
                    Name = e.Name;
                    break;
                case Events.BrandNameChanged e:
                    Name = e.Name;
                    break;
                case Events.BrandDeleted e:
                    Id = new BrandId(e.Id);
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