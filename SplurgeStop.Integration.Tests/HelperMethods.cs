﻿using System;
using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.UI.WebApi.Controllers;
using transaction = SplurgeStop.Domain.PurchaseTransaction;

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

            var command = new Commands.Create();
            command.Transaction = transaction;

            // Create PurchaseTransaction
            var transactionController = new PurchaseTransactionController(service);
            var result = await transactionController.Post(command);

            // Add PurchaseDate
            var updateCommand = new Commands.SetPurchaseTransactionDate();
            updateCommand.Id = transaction.Id;
            updateCommand.TransactionDate = DateTime.Now;
            await transactionController.Put(updateCommand);

            // Add Store
            var updateStoreCommand = new Commands.SetPurchaseTransactionStore();
            updateStoreCommand.Id = transaction.Id;
            updateStoreCommand.Store = new Store();
            updateStoreCommand.Store.Name = "Test market";
            await transactionController.Put(updateStoreCommand);

            // Add one LineItem
            var updateLineItemCommand = new Commands.SetPurchaseTransactionLineItem();
            updateLineItemCommand.Id = transaction.Id;
            updateLineItemCommand.LineItem = LineItemBuilder
                .LineItem(new Price(price, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end))
                .WithNotes(lineItem?.Notes)
                .Build();
            await transactionController.Put(updateLineItemCommand);

            return command.Transaction.Id;
        }

        public async static Task UpdatePurchaseDate(PurchaseTransactionId id, DateTime date, SplurgeStopDbContext context)
        {
            var repository = new PurchaseTransactionRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new PurchaseTransactionService(repository, unitOfWork);
            var transactionController = new PurchaseTransactionController(service);

            var updateCommand = new Commands.SetPurchaseTransactionDate();
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

            var updateCommand = new Commands.SetPurchaseTransactionLineItem();
            updateCommand.Id = id;
            updateCommand.LineItem = lineItem;

            await transactionController.Put(updateCommand);
        }

        public async static Task UpdateStoreName(PurchaseTransactionId id, string name, SplurgeStopDbContext context)
        {
            var repository = new PurchaseTransactionRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new PurchaseTransactionService(repository, unitOfWork);
            var transactionController = new PurchaseTransactionController(service);

            var updateCommand = new Commands.SetPurchaseTransactionStore();
            updateCommand.Id = id;
            updateCommand.Store = new Store();
            updateCommand.Store.Name = name;

            await transactionController.Put(updateCommand);
        }
    }
}
