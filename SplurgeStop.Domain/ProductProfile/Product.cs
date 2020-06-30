using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using SplurgeStop.Domain.ProductProfile.BrandProfile;

namespace SplurgeStop.Domain.ProductProfile
{
    public sealed class Product
    {
        public ProductId Id { get; set; }

        public Brand Brand { get; set; }
        public string Name { get; set; } //ProductName
        //public GeneralName GeneralName { get; set; }

        //public Size Size { get; set; }

    }
}
