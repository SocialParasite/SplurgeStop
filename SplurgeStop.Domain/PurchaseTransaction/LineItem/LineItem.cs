﻿using System;
using GuidHelpers;
using SplurgeStop.Domain.ProductProfile;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public class LineItem
    {
        internal LineItem(LineItemId id)
        {
            Id = id;
        }

        public LineItemId Id { get; }

        public Price Price { get; private set; }

        // TEMP product
        [Obsolete]
        public string TEMPProductName { get; set; }

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
        public LineItemId Id { get; set; }
        public Price Price { get; private set; }
        public string Notes { get; private set; }
        public string Product { get; set; }

        public static LineItemBuilder LineItem(Price price, LineItemId id = null)
        {
            return new LineItemBuilder { Price = price, Id = id };
        }

        public LineItemBuilder WithProduct(string product)
        {
            Product = product ?? string.Empty;
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
            lineItem.TEMPProductName = Product ?? string.Empty;

            return lineItem;
        }
    }

}
