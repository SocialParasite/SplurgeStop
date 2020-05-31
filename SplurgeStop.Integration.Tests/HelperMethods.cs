using System;
using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.UI.WebApi.Controllers;
using transaction = SplurgeStop.Domain.PurchaseTransaction;
using store = SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Integration.Tests
{
    public static class HelperMethods
    {
        public async static Task<PurchaseTransactionId> CreateValidPurchaseTransaction(decimal price = 1.00m,
                                                                                       LineItem lineItem = null)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new PurchaseTransactionRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new PurchaseTransactionService(repository, unitOfWork);

            var command = new transaction.Commands.Create();
            command.Id = null;

            // Create PurchaseTransaction
            var transactionController = new PurchaseTransactionController(service);
            var result = await transactionController.Post(command);

            // Add PurchaseDate
            await UpdatePurchaseDate(result.Value.Id, DateTime.Now);

            var purchaseTransaction = await repository.GetPurchaseTransactionFullAsync(result.Value.Id);

            // Add Store
            var store = await CreateValidStore();
            await UpdateStore(purchaseTransaction.Id, store);

            // Add one LineItem
            var updateLineItemCommand = new transaction.Commands.SetPurchaseTransactionLineItem();
            updateLineItemCommand.Id = purchaseTransaction.Id;
            updateLineItemCommand.Price = price;
            updateLineItemCommand.Currency = "EUR";
            updateLineItemCommand.CurrencySymbol = "€";
            updateLineItemCommand.Booking = Booking.Credit;
            updateLineItemCommand.CurrencySymbolPosition = CurrencySymbolPosition.end;
            updateLineItemCommand.Notes = lineItem?.Notes;
            //updateLineItemCommand.LineItem = LineItemBuilder
            //    .LineItem(new Price(price, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end))
            //    .WithNotes(lineItem?.Notes)
            //    .Build();
            await transactionController.Put(updateLineItemCommand);

            return command.Id;
        }

        public async static Task<Store> CreateValidStore()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);

            var command = new store.Commands.Create();
            command.Id = null;

            // Create Store
            var storeController = new StoreController(service);
            var storeId = await storeController.Post(command);

            // Update store name
            var updateCommand = new store.Commands.SetStoreName();
            updateCommand.Id = storeId.Value.Id;
            updateCommand.Name = "Test market";

            await storeController.Put(updateCommand);

            //return command.Store;
            return await repository.GetStoreFullAsync(command.Id);
        }

        public async static Task UpdatePurchaseDate(PurchaseTransactionId id, DateTime date)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new PurchaseTransactionRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new PurchaseTransactionService(repository, unitOfWork);
            var transactionController = new PurchaseTransactionController(service);

            var updateCommand = new transaction.Commands.SetPurchaseTransactionDate();
            updateCommand.Id = id;
            updateCommand.TransactionDate = date;

            await transactionController.Put(updateCommand);
        }

        public async static Task UpdateLineItem(PurchaseTransactionId id, LineItem lineItem)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new PurchaseTransactionRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new PurchaseTransactionService(repository, unitOfWork);
            var transactionController = new PurchaseTransactionController(service);

            var updateCommand = new transaction.Commands.SetPurchaseTransactionLineItem();
            updateCommand.Id = id;
            //updateCommand.LineItem = lineItem;
            updateCommand.Price = lineItem.Price.Amount;
            updateCommand.Currency = lineItem.Price.Currency.CurrencyCode;
            updateCommand.CurrencySymbol = lineItem.Price.Currency.CurrencySymbol;
            updateCommand.Booking = lineItem.Price.Booking;
            updateCommand.CurrencySymbolPosition = lineItem.Price.Currency.PositionRelativeToPrice;
            updateCommand.Notes = lineItem?.Notes;

            await transactionController.Put(updateCommand);
        }

        public async static Task UpdateStore(PurchaseTransactionId id, Store store)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new PurchaseTransactionRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new PurchaseTransactionService(repository, unitOfWork);
            var transactionController = new PurchaseTransactionController(service);

            var updateCommand = new transaction.Commands.SetPurchaseTransactionStore();
            updateCommand.Id = id;
            updateCommand.StoreId = store.Id;

            await transactionController.Put(updateCommand);
        }

        public async static Task UpdateStoreName(StoreId id, string name)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);
            var storeController = new StoreController(service);

            var updateCommand = new store.Commands.SetStoreName();
            updateCommand.Id = id;
            updateCommand.Name = name;

            await storeController.Put(updateCommand);
        }

        public async static Task<PurchaseTransaction> ReloadPurchaseTransaction(PurchaseTransactionId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new PurchaseTransactionRepository(context);

            return await repository.GetPurchaseTransactionFullAsync(id);
        }
    }
}
