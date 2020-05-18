using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplurgeStop.Domain.PurchaseTransaction;

namespace SplurgeStop.Data.EF
{
    public sealed class LineItemConfig : IEntityTypeConfiguration<LineItem>
    {
        public void Configure(EntityTypeBuilder<LineItem> builder)
        {
            builder.Property(x => x.Id)
                   .HasConversion(li => li.Value, g => g)
                   .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnName("Price")
                .HasMaxLength(16)
                .HasConversion(
                    priceWithUnit
                        => new string($"{priceWithUnit.Amount} | {priceWithUnit.CurrencyCode}"),
                     price => new Price
                     {
                         Amount = GetAmount(price),
                         CurrencyCode = GetCurrencyCode(price)
                     });
        }

        decimal GetAmount(string price) 
            => decimal.Parse(price.Substring(0, price.IndexOf('|', StringComparison.Ordinal)));

        string GetCurrencyCode(string price)
            => price.Substring(price.LastIndexOf("|", StringComparison.Ordinal) + 1).Trim();
    }
}
