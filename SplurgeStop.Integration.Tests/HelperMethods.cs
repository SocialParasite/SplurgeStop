using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.PurchaseTransaction.DTO;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.UI.WebApi.Controllers;
using store = SplurgeStop.Domain.StoreProfile;
using transaction = SplurgeStop.Domain.PurchaseTransaction;

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

            await transactionController.Put(updateLineItemCommand);

            return command.Id;
        }

        public async static Task<PurchaseTransactionId> CreateFullValidPurchaseTransaction()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new PurchaseTransactionRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new PurchaseTransactionService(repository, unitOfWork);

            var command = new transaction.Commands.CreateFull();
            command.Id = null;

            // Add PurchaseDate
            command.TransactionDate = DateTime.Now;

            // Add Store
            var store = await CreateValidStore();
            command.StoreId = store.Id;

            // Add one LineItem
            var newLineItem = new LineItemStripped
            {
                Booking = Booking.Credit,
                Price = "1.23",
                CurrencyCode = "EUR",
                CurrencySymbol = "€",
                CurrencySymbolPosition = CurrencySymbolPosition.end,
                Notes = "New notes"
            };

            command.LineItems = new List<LineItemStripped>();
            command.LineItems.Add(newLineItem);

            var transactionController = new PurchaseTransactionController(service);
            var result = await transactionController.Post(command);

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
            command.Name = "New store";
            command.Id = null;

            // Create Store
            var storeController = new StoreController(service);
            var storeId = await storeController.Post(command);

            // Update store name
            var updateCommand = new store.Commands.SetStoreName();
            updateCommand.Id = storeId.Value.Id;
            updateCommand.Name = "Test market";

            await storeController.Put(updateCommand);

            return await repository.GetStoreFullAsync(command.Id);
        }

        public async static Task<dynamic> CreateInvalidStore()
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
            return await storeController.Post(command);
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
            updateCommand.LineItemId = lineItem.Id;
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

        public async static Task RemoveStore(StoreId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);
            var storeController = new StoreController(service);

            var updateCommand = new store.Commands.DeleteStore();
            updateCommand.Id = id;

            await storeController.DeleteStore(updateCommand);
        }

        public async static Task<PurchaseTransaction> ReloadPurchaseTransaction(PurchaseTransactionId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new PurchaseTransactionRepository(context);

            return await repository.GetPurchaseTransactionFullAsync(id);
        }

        public async static Task<bool> CheckIfStoreExists(StoreId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new StoreRepository(context);

            return await repository.ExistsAsync(id);
        }
    }
}
