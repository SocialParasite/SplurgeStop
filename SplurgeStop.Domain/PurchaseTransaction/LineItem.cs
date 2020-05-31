using System;
using GuidHelpers;

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

        //public int Quantitity { get; set; } // this will always be pieces; other "measurements" in Product 
        //(change type? Could be g, l, kg, pieces)

        public void UpdateLineItemPrice(Price newPrice)
        {
            Price = newPrice ?? throw new ArgumentNullException("Price is required.");
        }

        public string Notes { get; set; }
    }

    public class LineItemBuilder
    {
        public LineItemId Id { get; set; }
        public Price Price { get; private set; }
        public string Notes { get; private set; }

        public static LineItemBuilder LineItem(Price price, LineItemId id = null)
        {
            return new LineItemBuilder { Price = price, Id = id };
        }

        public LineItemBuilder WithNotes(string notes)
        {
            Notes = notes ?? string.Empty;
            return this;
        }

        public LineItem Build()
        {
            LineItem lineItem;

            if (Id is null)
            {
                lineItem = new LineItem(new LineItemId(SequentialGuid.NewSequentialGuid()));
            }
            else
            {
                lineItem = new LineItem(Id);
            }

            lineItem.UpdateLineItemPrice(Price);
            lineItem.Notes = Notes ?? string.Empty;

            return lineItem;
        }
    }

}
