using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplurgeStop.Domain.ProductProfile;

namespace SplurgeStop.Data.EF
{
    public sealed class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id)
                   .HasConversion(li => li.Value, g => g)
                   .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.HasOne(x => x.Brand);
            builder.HasOne(x => x.ProductType);
            builder.HasOne(x => x.Size);
        }
    }
}
