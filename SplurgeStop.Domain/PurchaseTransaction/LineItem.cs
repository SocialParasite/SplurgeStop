using System;
using System.Collections.Generic;
using System.Text;
using GuidHelpers;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public sealed class LineItem
    {
        public LineItemId Id { get; }

        public LineItem()
        {
            Id = new LineItemId(SequentialGuid.NewSequentialGuid());
        }

        //public LineItem(Product product, Price price)
        //{

        //}

    }
}
