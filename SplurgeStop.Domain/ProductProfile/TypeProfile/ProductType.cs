using System;

namespace SplurgeStop.Domain.ProductProfile.TypeProfile
{
    public class ProductType
    {
        private ProductType() { }

        public ProductTypeId Id { get; set; }
        public string Name { get; set; }

        public static ProductType Create(ProductTypeId id, string name)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id), "Product type without unique identifier cannot be created.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Product type without name cannot be created.");

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
            if (name is null)
                throw new ArgumentNullException(nameof(name), "A valid name for product type must be provided.");

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