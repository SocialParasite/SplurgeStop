using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.LineItemProfile;
using SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IPurchaseTransactionRepository
        : IRepository<PurchaseTransaction, PurchaseTransactionStripped, PurchaseTransactionId>
    {
        Task<PurchaseTransaction> LoadFullPurchaseTransactionAsync(PurchaseTransactionId id);
        Task<Product> GetProductAsync(ProductId id);
        Task<Store> GetStoreAsync(StoreId id);
        Task ChangeStore(PurchaseTransaction purchaseTransaction, StoreId storeId);
        Task ChangeLineItem(PurchaseTransaction purchaseTransaction, LineItem lineItem);
    }
}
