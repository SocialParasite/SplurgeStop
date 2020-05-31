using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplurgeStop.Domain.PurchaseTransaction;

namespace SplurgeStop.Data.EF
{
    public sealed class PurchaseTransactionConfig : IEntityTypeConfiguration<PurchaseTransaction>
    {
        public void Configure(EntityTypeBuilder<PurchaseTransaction> builder)
        {
            builder.Property(x => x.Id)
                   .HasConversion(ti => ti.Value, g => g)
                   .IsRequired();

            builder.Property(d => d.PurchaseDate)
                   .HasConversion(pd => pd.Value, d => d)
                   .HasColumnType("Date")
                   .IsRequired();

            builder.Property(n => n.Notes)
                   .HasConversion(pn => pn.Value, n => n)
                   .HasMaxLength(500);

            builder.HasOne(s => s.Store);
        }
    }
}
