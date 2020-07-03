using System;
using System.Threading.Tasks;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;

namespace SplurgeStop.Domain.ProductProfile
{
    public sealed class Product
    {
        public ProductId Id { get; set; }

        public Brand Brand { get; set; }
        public string Name { get; set; }

        public ProductType ProductType { get; set; }
        //public GeneralName GeneralName { get; set; }

        public Size Size { get; set; }

        public static Product Create(ProductId id, string name, Brand brand)
        {
            var product = new Product();

            product.Apply(new Events.ProductCreated
            {
                Id = id,
                Name = name,
                Brand = brand
            });

            return product;
        }

        internal void UpdateProductName(string name)
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
                case Events.ProductCreated e:
                    Id = new ProductId(e.Id);
                    Name = e.Name;
                    Brand = e.Brand;
                    break;
                case Events.ProductNameChanged e:
                    Name = e.Name;
                    break;
                case Events.BrandChanged e:
                    Id = new ProductId(e.Id);
                    Brand = e.Brand;
                    break;
                case Events.ProductDeleted e:
                    Id = new ProductId(e.Id);
                    Name = e.Name;
                    break;
            }
        }

        internal bool EnsureValidState()
        {
            return Id.Value != default
                   && !string.IsNullOrWhiteSpace(Name);
        }

        public void UpdateBrand(Brand brand)
        {
            Brand = brand;
        }
    }
}
