using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.PurchaseTransaction.LineItemProfile;
using SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Data.EF.Repositories
{
    public sealed class PurchaseTransactionRepository : IPurchaseTransactionRepository
    {
        private readonly SplurgeStopDbContext _context;

        public PurchaseTransactionRepository(SplurgeStopDbContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddPurchaseTransactionAsync(PurchaseTransaction transaction)
        {
            await _context.Purchases.AddAsync(transaction);
        }

        public async Task<bool> ExistsAsync(PurchaseTransactionId id)
        {
            return await _context.Purchases.FindAsync(id) != null;
        }

        public async Task<PurchaseTransaction> GetPurchaseTransactionFullAsync(PurchaseTransactionId id)
        {
            return await _context.Purchases
                .Include(p => p.Store)
                .Include(p => p.LineItems)
                .ThenInclude(l => l.Product)
                .ThenInclude(p => p.Brand)
                .Include(p => p.LineItems)
                .ThenInclude(l => l.Product)
                .ThenInclude(p => p.ProductType)
                .Include(p => p.LineItems)
                .ThenInclude(l => l.Product)
                .ThenInclude(p => p.Size)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PurchaseTransactionStripped>> GetAllPurchaseTransactionsAsync()
        {
            return await _context.Purchases
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
            return await _context.Purchases.FindAsync(id);
        }

        public async Task<PurchaseTransaction> LoadFullPurchaseTransactionAsync(PurchaseTransactionId id)
        {
            return await _context.Purchases.Include(s => s.Store)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Store> GetStoreAsync(StoreId id)
        {
            return await _context.Stores.FindAsync(id);
        }

        public async Task ChangeStore(PurchaseTransaction purchaseTransaction, StoreId storeId)
        {
            var transaction = await _context.Purchases.FindAsync(purchaseTransaction.Id);
            transaction.UpdateStore(await GetStoreAsync(storeId));
            await _context.SaveChangesAsync();
        }

        public async Task ChangeLineItem(PurchaseTransaction purchaseTransaction, LineItem lineItem)
        {
            var transaction = await _context.Purchases
                .Include(p => p.LineItems)
                .ThenInclude(l => l.Product)
                .ThenInclude(p => p.Brand)
                .Include(p => p.LineItems)
                .ThenInclude(l => l.Product)
                .ThenInclude(p => p.ProductType)
                .Include(p => p.LineItems)
                .ThenInclude(l => l.Product)
                .ThenInclude(p => p.Size)
                .FirstOrDefaultAsync(p => p.Id == purchaseTransaction.Id);

            LineItem toBeRemoved = transaction.LineItems.Find(l => l.Id == lineItem.Id);
            purchaseTransaction.LineItems.Remove(toBeRemoved);
            await _context.SaveChangesAsync();

            lineItem.Product = await _context.Products.FindAsync(lineItem.Product.Id);
            purchaseTransaction.LineItems.Add(lineItem);

            await _context.SaveChangesAsync();
        }

        public async Task RemovePurchaseTransactionAsync(PurchaseTransactionId id)
        {
            var pt = await _context.Purchases.FindAsync(id);

            if (pt != null)
                _context.Purchases.Remove(pt);
        }

        public async Task<Product> GetProductAsync(ProductId id)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductType)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
