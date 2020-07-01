using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.PurchaseTransaction.DTO;
using SplurgeStop.Domain.StoreProfile;

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
                .Include(p => p.LineItems)
                .ThenInclude(l => l.Product)
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
                        TotalPrice = r.TotalPrice,
                        ItemCount = r.LineItems.Count
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

        public async Task<Store> GetStoreAsync(StoreId id)
        {
            return await context.Stores.FindAsync(id);
        }

        public async Task ChangeStore(PurchaseTransaction purchaseTransaction, StoreId storeId)
        {
            var transaction = await context.Purchases.FindAsync(purchaseTransaction.Id);
            transaction.UpdateStore(await GetStoreAsync(storeId));
            await context.SaveChangesAsync();
        }

        public async Task ChangeLineItem(PurchaseTransaction purchaseTransaction, LineItem lineItem)
        {
            var transaction = await context.Purchases
                .Include(p => p.LineItems)
                .ThenInclude(l => l.Product)
                .FirstOrDefaultAsync(p => p.Id == purchaseTransaction.Id);

            LineItem toBeRemoved = transaction.LineItems.Find(l => l.Id == lineItem.Id);
            purchaseTransaction.LineItems.Remove(toBeRemoved);
            await context.SaveChangesAsync();

            purchaseTransaction.LineItems.Add(lineItem);
            await context.SaveChangesAsync();
        }

        public async Task RemovePurchaseTransactionAsync(PurchaseTransactionId id)
        {
            var pt = await context.Purchases.FindAsync(id);

            if (pt != null)
                context.Purchases.Remove(pt);
        }
    }
}
