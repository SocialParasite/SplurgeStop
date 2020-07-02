using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplurgeStop.Domain.LocationProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;

namespace SplurgeStop.Data.EF
{
    public sealed class SizeConfig : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> builder)
        {
            builder.Property(x => x.Id)
                .HasConversion(l => l.Value, g => g)
                .IsRequired();
        }
    }
}
