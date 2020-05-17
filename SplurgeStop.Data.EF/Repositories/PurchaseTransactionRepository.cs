using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.PurchaseTransaction;
using System.Linq;
using System.Collections.Generic;
using SplurgeStop.Domain.PurchaseTransaction.DTO;

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

        public async Task<PurchaseTransaction> GetPurchaseTransactionFullAsync(PurchaseTransactionId id)
        {
            return await context.Purchases
                .Include(p => p.Store)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PurchaseTransactionStripped>> GetAllPurchaseTransactionsAsync()
        {
            return await context.Purchases
                    .Include(s => s.Store)
                    .Include(l => l.LineItems)
                    .Select(r => new PurchaseTransactionStripped
                    {
                        Id = r.Id,
                        StoreName = r.Store.Name,
                        PurchaseDate = r.PurchaseDate.Value,
                        TotalPrice = r.TotalPrice.ElementAt(0).TotalSum,
                        ItemCount = r.LineItems.Count()
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
