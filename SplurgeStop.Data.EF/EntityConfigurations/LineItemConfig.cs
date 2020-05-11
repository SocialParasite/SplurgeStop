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
            => decimal.Parse(price.Substring(0, Int32.Parse(ExtensionMethods.GetUntilOrEmpty("|"))).Trim());

        string GetCurrencyCode(string price) 
            => price.Substring(Int32.Parse(ExtensionMethods.GetUntilOrEmpty("|") + 1));
    }

    public static class ExtensionMethods
    {
        public static string GetUntilOrEmpty(this string text, string stopAt = "-")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }
    }

}
