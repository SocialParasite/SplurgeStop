using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.PurchaseTransaction;
using System.Linq;
using System.Collections.Generic;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class PurchaseTransactionRepository : IPurchaseTransactionRepository
    {
        private readonly SplurgeStopDbContext context;

        public PurchaseTransactionRepository(SplurgeStopDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddPurchaseTransactionAsync(PurchaseTransaction transaction)
        {
            await context.Purchases.AddAsync(transaction);
        }

        public async Task<bool> ExistsAsync(PurchaseTransactionId id)
        {
            return await context.Purchases.FindAsync(id) != null; 
        }

        public async Task<PurchaseTransaction> GetAllPurchaseTransactionAsync(PurchaseTransactionId id)
        {
            return await context.Purchases
                .Include(p => p.Store)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        //public async Task<IEnumerable<PurchaseTransaction>> GetAllPurchaseTransactionsAsync()
        public async Task<IEnumerable<object>> GetAllPurchaseTransactionsAsync()
        {
            //return await context.Purchases
            //    .Include(s => s.Store)
            //    .Select(s => s)
            //    .AsNoTracking()
            //    .ToListAsync();

            return await context.Purchases
                    .Include(s => s.Store)
                    .Include(l => l.LineItems)
                    .Select(r => new
                    {
                        r.Id,
                        r.Store.Name,
                        r.PurchaseDate,
                        r.TotalPrice
                        // Total number of LineItems
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<PurchaseTransaction> LoadPurchaseTransactionAsync(PurchaseTransactionId id)
        {
            return await context.Purchases.FindAsync(id);
        }

        public async Task<PurchaseTransaction> LoadFullPurchaseTransactionAsync(PurchaseTransactionId id)
        {
            return await context.Purchases.Include(s => s.Store).FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
