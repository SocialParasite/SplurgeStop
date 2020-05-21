using System;

namespace SplurgeStop.Domain.PurchaseTransaction.DTO
{
    public class PurchaseTransactionStripped
    {
        public Guid Id { get; set; }
        public string StoreName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string TotalPrice { get; set; }
        public int ItemCount { get; set; }
    }
}
