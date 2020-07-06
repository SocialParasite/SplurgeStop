using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.LineItemProfile;
using SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IPurchaseTransactionRepository
    {
        Task AddPurchaseTransactionAsync(PurchaseTransaction transaction);
        Task<bool> ExistsAsync(PurchaseTransactionId id);
        Task<PurchaseTransaction> LoadPurchaseTransactionAsync(PurchaseTransactionId id);
        Task<IEnumerable<PurchaseTransactionStripped>> GetAllPurchaseTransactionsAsync();
        Task<PurchaseTransaction> GetPurchaseTransactionFullAsync(PurchaseTransactionId id);

        Task<Store> GetStoreAsync(StoreId id);
        Task ChangeStore(PurchaseTransaction purchaseTransaction, StoreId storeId);

        Task ChangeLineItem(PurchaseTransaction purchaseTransaction, LineItem lineItem);

        Task RemovePurchaseTransactionAsync(PurchaseTransactionId id);
        Task<Product> GetProductAsync(ProductId id);
    }
}
