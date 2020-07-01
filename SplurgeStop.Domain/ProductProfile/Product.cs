using SplurgeStop.Domain.ProductProfile.BrandProfile;
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

        //public Size Size { get; set; }
    }
}
