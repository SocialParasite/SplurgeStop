using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplurgeStop.Domain.PurchaseTransactionProfile.LineItemProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.PriceProfile;

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
                .HasMaxLength(32)
                .HasConversion(priceDetails
                    => new string($"{priceDetails.Amount};" +
                                  $"{priceDetails.Booking};" +
                                  $"{priceDetails.Currency.CurrencyCode};" +
                                  $"{priceDetails.Currency.CurrencySymbol};" +
                                  $"{priceDetails.Currency.PositionRelativeToPrice}"),
                    price
                        => new Price(decimal.Parse(GetSection(price, 0)),
                                     (Booking)Enum.Parse(typeof(Booking), GetSection(price, 1)),
                                     GetSection(price, 2),
                                     GetSection(price, 3),
                                     (CurrencySymbolPosition)Enum.Parse(typeof(CurrencySymbolPosition), GetSection(price, 4))));

            builder.HasOne(x => x.PurchaseTransaction)
                .WithMany(x => x.LineItems).IsRequired();

            builder.HasOne(x => x.Product);
        }

        string GetSection(string text, int pos)
            => text.Split(';')[pos];
    }
}
