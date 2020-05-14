namespace SplurgeStop.Domain.PurchaseTransaction
{
    public struct PurchaseTotalSum
    {
        public CurrencyCode CurrencyCode { get; set; }
        public decimal TotalSum { get; set; }
    }
}
