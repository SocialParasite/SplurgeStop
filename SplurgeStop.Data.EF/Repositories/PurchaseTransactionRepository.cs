using System;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.PurchaseTransaction;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class PurchaseTransactionRepository : IPurchaseTransactionRepository
    {
        private readonly SplurgeStopDbContext context;

        public PurchaseTransactionRepository(SplurgeStopDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(PurchaseTransaction transaction)
        {
            await context.Purchases.AddAsync(transaction);
        }

        public async Task<bool> Exists(TransactionId id)
        {
            return await context.Purchases.FindAsync(id) != null; 
        }

        public async Task<PurchaseTransaction> Load(TransactionId id)
        {
            throw new NotImplementedException();
        }
    }
}
