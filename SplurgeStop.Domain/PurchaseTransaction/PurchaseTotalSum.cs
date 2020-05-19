namespace SplurgeStop.Domain.PurchaseTransaction
{
    public struct PurchaseTotalSum
    {
        public Currency Currency { get; set; }
        public decimal TotalSum { get; set; }
    }
}
