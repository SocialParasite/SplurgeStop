using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.PurchaseTransaction.DTO;
using transaction = SplurgeStop.Domain.PurchaseTransaction;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IPurchaseTransactionRepository
    {
        Task AddPurchaseTransactionAsync(transaction.PurchaseTransaction transaction);
        Task<bool> ExistsAsync(transaction.PurchaseTransactionId id);
        Task<transaction.PurchaseTransaction> LoadPurchaseTransactionAsync(transaction.PurchaseTransactionId id);
        //Task<IEnumerable<transaction.PurchaseTransaction>> GetAllPurchaseTransactionsAsync();
        Task<IEnumerable<PurchaseTransactionStripped>> GetAllPurchaseTransactionsAsync();
        Task<transaction.PurchaseTransaction> GetPurchaseTransactionFullAsync(PurchaseTransactionId id);
    }
}
