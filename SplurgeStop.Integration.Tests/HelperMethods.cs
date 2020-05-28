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
        public async static Task<PurchaseTransactionId> CreateValidPurchaseTransaction(SplurgeStopDbContext context,
            decimal price = 1.00m,
            LineItem lineItem = null)
        {
            var repository = new PurchaseTransactionRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new PurchaseTransactionService(repository, unitOfWork);
            var transaction = new PurchaseTransaction();

            var command = new transaction.Commands.Create();
            command.Transaction = transaction;

            // Create PurchaseTransaction
            var transactionController = new PurchaseTransactionController(service);
            var result = await transactionController.Post(command);

            // Add PurchaseDate
            var updateCommand = new transaction.Commands.SetPurchaseTransactionDate();
            updateCommand.Id = transaction.Id;
            updateCommand.TransactionDate = DateTime.Now;
            await transactionController.Put(updateCommand);

            // Add Store
            var updateStoreCommand = new transaction.Commands.SetPurchaseTransactionStore();
            updateStoreCommand.Id = transaction.Id;
            //updateStoreCommand.Store = new Store();
            //updateStoreCommand.Store.Name = "Test market";
            updateStoreCommand.Store = await CreateValidStore(context);
            await transactionController.Put(updateStoreCommand);

            // Add one LineItem
            var updateLineItemCommand = new transaction.Commands.SetPurchaseTransactionLineItem();
            updateLineItemCommand.Id = transaction.Id;
            updateLineItemCommand.LineItem = LineItemBuilder
                .LineItem(new Price(price, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end))
                .WithNotes(lineItem?.Notes)
                .Build();
            await transactionController.Put(updateLineItemCommand);

            return command.Transaction.Id;
        }

        public async static Task<Store> CreateValidStore(SplurgeStopDbContext context)
        {
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);
            var store = new Store();

            var command = new store.Commands.Create();
            command.Store = store;

            // Create Store
            var storeController = new StoreController(service);
            await storeController.Post(command);

            // Update store name
            var updateCommand = new store.Commands.SetStoreName();
            updateCommand.Id = command.Store.Id;
            updateCommand.Name = "Test market";

            await storeController.Put(updateCommand);

            return command.Store;
            //return await repository.GetStoreFullAsync(command.Store.Id);
        }

        public async static Task UpdatePurchaseDate(PurchaseTransactionId id, DateTime date, SplurgeStopDbContext context)
        {
            var repository = new PurchaseTransactionRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new PurchaseTransactionService(repository, unitOfWork);
            var transactionController = new PurchaseTransactionController(service);

            var updateCommand = new transaction.Commands.SetPurchaseTransactionDate();
            updateCommand.Id = id;
            updateCommand.TransactionDate = date;

            await transactionController.Put(updateCommand);
        }

        public async static Task UpdateLineItems(PurchaseTransactionId id,
            LineItem lineItem,
            SplurgeStopDbContext context)
        {
            var repository = new PurchaseTransactionRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new PurchaseTransactionService(repository, unitOfWork);
            var transactionController = new PurchaseTransactionController(service);

            var updateCommand = new transaction.Commands.SetPurchaseTransactionLineItem();
            updateCommand.Id = id;
            updateCommand.LineItem = lineItem;

            await transactionController.Put(updateCommand);
        }

        //public async static Task UpdateStoreName(PurchaseTransactionId id, string name, SplurgeStopDbContext context)
        //{
        //    var repository = new PurchaseTransactionRepository(context);
        //    var unitOfWork = new EfCoreUnitOfWork(context);
        //    var service = new PurchaseTransactionService(repository, unitOfWork);
        //    var transactionController = new PurchaseTransactionController(service);

        //    var updateCommand = new transaction.Commands.SetPurchaseTransactionStore();
        //    updateCommand.Id = id;
        //    updateCommand.Store = new Store();
        //    updateCommand.Store.Name = name;

        //    await transactionController.Put(updateCommand);
        //}
        public async static Task UpdateStoreName(StoreId id, string name, SplurgeStopDbContext context)
        {
            var repository = new StoreRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new StoreService(repository, unitOfWork);
            var storeController = new StoreController(service);

            var updateCommand = new store.Commands.SetStoreName();
            updateCommand.Id = id;
            updateCommand.Name = name;

            await storeController.Put(updateCommand);
        }

    }
}
