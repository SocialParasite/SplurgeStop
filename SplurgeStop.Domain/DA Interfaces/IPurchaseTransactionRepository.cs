using System.Threading.Tasks;
using transaction = SplurgeStop.Domain.PurchaseTransaction;

namespace SplurgeStop.Domain.DA_Interfaces
{
    public interface IPurchaseTransactionRepository
    {
        Task Add(transaction.PurchaseTransaction transaction);
        Task<bool> Exists(transaction.PurchaseTransactionId id);
        Task<transaction.PurchaseTransaction> Load(transaction.PurchaseTransactionId id);
    }
}
