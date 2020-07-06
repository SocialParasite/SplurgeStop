using System;
using GuidHelpers;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.PriceProfile;

namespace SplurgeStop.Domain.PurchaseTransactionProfile.LineItemProfile
{
    public class LineItem
    {
        internal LineItem(LineItemId id)
        {
            Id = id;
        }

        public LineItemId Id { get; }

        public Price Price { get; private set; }

        public int Quantity { get; set; }
        public string Notes { get; set; }

        public Product Product { get; set; }
        public PurchaseTransaction PurchaseTransaction { get; set; }

        public void UpdateLineItemPrice(Price newPrice)
        {
            Price = newPrice ?? throw new ArgumentNullException(nameof(newPrice), "Price is required.");
        }
    }

    public class LineItemBuilder
    {
        private LineItemId Id { get; set; }
        private Price Price { get; set; }
        private string Notes { get; set; }

        private Product Product { get; set; }

        public static LineItemBuilder LineItem(Price price, LineItemId id = null)
        {
            return new LineItemBuilder { Price = price, Id = id };
        }

        public LineItemBuilder WithProduct(Product product)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product), "Invalid product provided.");
            return this;
        }

        public LineItemBuilder WithNotes(string notes)
        {
            Notes = notes ?? string.Empty;
            return this;
        }

        public LineItem Build()
        {
            var lineItem = Id is null
                ? new LineItem(new LineItemId(SequentialGuid.NewSequentialGuid()))
                : new LineItem(Id);

            lineItem.UpdateLineItemPrice(Price);
            lineItem.Notes = Notes ?? string.Empty;
            lineItem.Product = Product ?? throw new ArgumentNullException(nameof(Product), "Invalid product provided.");

            return lineItem;
        }
    }

}
