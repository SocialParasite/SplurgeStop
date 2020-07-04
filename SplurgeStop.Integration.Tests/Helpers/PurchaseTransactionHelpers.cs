using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.PurchaseTransaction.DTO;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.UI.WebApi.Controllers;
using store = SplurgeStop.Domain.StoreProfile;
using transaction = SplurgeStop.Domain.PurchaseTransaction;

namespace SplurgeStop.Integration.Tests.Helpers
{
    public static class PurchaseTransactionHelpers
    {
        public static async Task<PurchaseTransactionId> CreateValidPurchaseTransaction(decimal price = 1.00m,
                                                                                      LineItem lineItem = null)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new PurchaseTransactionRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new PurchaseTransactionService(repository, unitOfWork);

            var command = new transaction.Commands.CreateFull();
            command.Id = null;
            command.TransactionDate = DateTime.Now;

            // Add Store
            var store = await StoreHelpers.CreateValidStore();
            command.StoreId = store.Id;

            // Get Product
            var prod = await ProductHelpers.CreateValidProduct();

            // Add one LineItem
            command.LineItems = new List<LineItemStripped>();
            command.LineItems.Add(new LineItemStripped
            {
                Price = price.ToString(),
                CurrencyCode = "EUR",
                CurrencySymbol = "€",
                CurrencySymbolPosition = CurrencySymbolPosition.end,
                Booking = Booking.Credit,
                Notes = lineItem?.Notes,
                Product = prod
            });

            // Create PurchaseTransaction
            var transactionController = new PurchaseTransactionController(service);
            await transactionController.Post(command);

            return command.Id;
        }

        public static async Task<PurchaseTransactionId> CreateFullValidPurchaseTransaction()
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
            var store = await StoreHelpers.CreateValidStore();
            command.StoreId = store.Id;

            // Get Product
            var prod = await ProductHelpers.CreateValidProduct();

            // Add one LineItem
            var newLineItem = new LineItemStripped
            {
                Booking = Booking.Credit,
                Price = "1.23",
                CurrencyCode = "EUR",
                CurrencySymbol = "€",
                CurrencySymbolPosition = CurrencySymbolPosition.end,
                Notes = "New notes",
                Product = prod
            };

            command.LineItems = new List<LineItemStripped>();
            command.LineItems.Add(newLineItem);

            var transactionController = new PurchaseTransactionController(service);
            await transactionController.Post(command);

            return command.Id;
        }

        public static async Task UpdatePurchaseDate(PurchaseTransactionId id, DateTime date)
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

        public static async Task UpdateLineItem(PurchaseTransactionId id, LineItem lineItem)
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
            updateCommand.Product = lineItem.Product;

            await transactionController.Put(updateCommand);
        }

        public static async Task UpdateStore(PurchaseTransactionId id, Store store)
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

        public static async Task<PurchaseTransaction> ReloadPurchaseTransaction(PurchaseTransactionId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new PurchaseTransactionRepository(context);

            return await repository.GetPurchaseTransactionFullAsync(id);
        }
    }
}
