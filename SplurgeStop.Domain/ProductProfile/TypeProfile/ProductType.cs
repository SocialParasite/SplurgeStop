using System;

namespace SplurgeStop.Domain.ProductProfile.TypeProfile
{
    public class ProductType
    {
        public ProductTypeId Id { get; set; }
        public string Name { get; set; }

        public static ProductType Create(ProductTypeId id, string name)
        {
            var productType = new ProductType();

            productType.Apply(new Events.ProductTypeCreated
            {
                Id = id,
                Name = name
            });

            return productType;
        }

        public void UpdateProductTypeName(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name), "A valid name for product type must be provided.");
        }

        private void Apply(object @event)
        {
            When(@event);
        }

        private void When(object @event)
        {
            switch (@event)
            {
                case Events.ProductTypeCreated e:
                    Id = new ProductTypeId(e.Id);
                    Name = e.Name;
                    break;
                case Events.ProductTypeNameChanged e:
                    Name = e.Name;
                    break;
                case Events.ProductTypeDeleted e:
                    Id = new ProductTypeId(e.Id);
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