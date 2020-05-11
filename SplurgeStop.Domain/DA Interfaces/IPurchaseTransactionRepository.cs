using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.PurchaseTransaction;
using transaction = SplurgeStop.Domain.PurchaseTransaction;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IPurchaseTransactionRepository
    {
        Task AddPurchaseTransactionAsync(transaction.PurchaseTransaction transaction);
        Task<bool> ExistsAsync(transaction.PurchaseTransactionId id);
        Task<transaction.PurchaseTransaction> LoadPurchaseTransactionAsync(transaction.PurchaseTransactionId id);
        //Task<IEnumerable<transaction.PurchaseTransaction>> GetAllPurchaseTransactionsAsync();
        Task<IEnumerable<object>> GetAllPurchaseTransactionsAsync();
        Task<transaction.PurchaseTransaction> GetAllPurchaseTransactionAsync(PurchaseTransactionId id);
    }
}
