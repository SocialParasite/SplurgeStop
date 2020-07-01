using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplurgeStop.Domain.ProductProfile.TypeProfile;

namespace SplurgeStop.Data.EF
{
    public sealed class ProductTypeConfig : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.Property(x => x.Id)
                   .HasConversion(c => c.Value, g => g)
                   .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128);
        }
    }
}
