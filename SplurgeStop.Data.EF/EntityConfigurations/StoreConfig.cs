using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Data.EF
{
    public sealed class StoreConfig : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.Property(x => x.Id)
                   .HasConversion(li => li.Value, g => g)
                   .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder.HasOne(x => x.Location);
        }
    }
}
