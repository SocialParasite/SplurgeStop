using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.PurchaseTransaction.LineItem;
using SplurgeStop.Domain.StoreProfile;
using transaction = SplurgeStop.Domain.PurchaseTransaction;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IPurchaseTransactionRepository
    {
        Task AddPurchaseTransactionAsync(transaction.PurchaseTransaction transaction);
        Task<bool> ExistsAsync(PurchaseTransactionId id);
        Task<transaction.PurchaseTransaction> LoadPurchaseTransactionAsync(transaction.PurchaseTransactionId id);
        Task<IEnumerable<PurchaseTransactionStripped>> GetAllPurchaseTransactionsAsync();
        Task<transaction.PurchaseTransaction> GetPurchaseTransactionFullAsync(PurchaseTransactionId id);

        Task<Store> GetStoreAsync(StoreId id);
        Task ChangeStore(transaction.PurchaseTransaction purchaseTransaction, StoreId storeId);

        Task ChangeLineItem(transaction.PurchaseTransaction purchaseTransaction, LineItem lineItem);

        Task RemovePurchaseTransactionAsync(PurchaseTransactionId id);
        Task<Product> GetProductAsync(ProductId id);
    }
}
