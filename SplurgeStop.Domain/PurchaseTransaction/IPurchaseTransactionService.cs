using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.PurchaseTransaction.DTO;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public interface IPurchaseTransactionService
    {
        Task<IEnumerable<PurchaseTransactionStripped>> GetAllPurchaseTransactions();
        Task<PurchaseTransaction> GetDetailedPurchaseTransaction(PurchaseTransactionId id);
        Task Handle(object command);
    }
}