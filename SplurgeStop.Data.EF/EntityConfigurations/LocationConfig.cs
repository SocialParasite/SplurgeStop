using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Data.EF
{
    public sealed class LocationConfig : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.Property(x => x.Id)
                   .HasConversion(l => l.Value, g => g)
                   .IsRequired();

            builder.HasOne(x => x.City);
            builder.HasOne(x => x.Country);
        }
    }
}
