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

        public Price Price { get; set; }        

        //public int Quantitity { get; set; } // change type? Could be g, l, kg, pieces
        //public string Notes { get; set; }
    }
}
