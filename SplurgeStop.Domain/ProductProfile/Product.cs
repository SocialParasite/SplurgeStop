using System;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;

namespace SplurgeStop.Domain.ProductProfile
{
    public class Product
    {
        private Product() { }

        public ProductId Id { get; set; }

        public Brand Brand { get; set; }
        public string Name { get; set; }

        public ProductType ProductType { get; set; }
        //public GeneralName GeneralName { get; set; }

        public Size Size { get; set; }

        public static Product Create(ProductId id, string name, Brand brand)
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id), "Product without unique identifier cannot be created.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Product without name cannot be created.");

            var product = new Product();

            product.Apply(new Events.ProductCreated
            {
                Id = id,
                Name = name,
                Brand = brand
            });

            return product;
        }

        public void UpdateProductName(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name), "A valid name for product type must be provided.");

            if (name.Length < 1 || name.Length > 128)
                throw new ArgumentException("Name length should be 128 characters or less, but not empty.", nameof(name));

            Name = name;
        }

        public void UpdateBrand(Brand brand)
        {
            Brand = brand ?? throw new ArgumentNullException(nameof(brand), "A valid brand must be provided.");
        }

        public void UpdateProductType(ProductType productType)
        {
            ProductType = productType ?? throw new ArgumentNullException(nameof(productType),
                "A valid product type must be provided.");
        }

        public void UpdateSize(Size size)
        {
            Size = size ?? throw new ArgumentNullException(nameof(size), "A valid size must be provided.");
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
                case Events.ProductTypeChanged e:
                    Id = new ProductId(e.Id);
                    ProductType = e.ProductType;
                    break;
                case Events.SizeChanged e:
                    Id = new ProductId(e.Id);
                    Size = e.Size;
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
    }
}
