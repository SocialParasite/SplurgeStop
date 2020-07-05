namespace SplurgeStop.Domain.PurchaseTransaction.PriceProfile
{
    public struct PurchaseTotalSum
    {
        public Currency Currency { get; set; }
        public decimal TotalSum { get; set; }
    }
}
