using System.Collections.Generic;
using System.Threading.Tasks;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public interface IPurchaseTransactionService
    {
        Task<IEnumerable<PurchaseTransactionStripped>> GetAllPurchaseTransactions();
        Task<PurchaseTransaction> GetDetailedPurchaseTransaction(PurchaseTransactionId id);
        Task Handle(object command);
    }
}