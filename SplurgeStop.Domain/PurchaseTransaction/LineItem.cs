using System;
using System.Collections.Generic;
using System.Text;
using GuidHelpers;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public class LineItem
    {

        public LineItem()
        {
            Id = new LineItemId(SequentialGuid.NewSequentialGuid());
        }

        public LineItemId Id { get; }

        // Store to LineItems table as two fields (Amount and CurrencyCode)
        public Price Price { get; set; }
        //public int Quantitity { get; set; } // change type? Could be g, l, kg, pieces
        //public string Notes { get; set; }

        //public PurchaseTransaction PurchaseTransaction { get; set; }
        // Product
        // price
        // notes
        // quantity

        //public LineItem(Product product, Price price)
        //{

        //}

    }
}
