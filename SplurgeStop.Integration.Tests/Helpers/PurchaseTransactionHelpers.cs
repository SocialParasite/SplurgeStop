using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.PurchaseTransactionProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.LineItemProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.PriceProfile;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.UI.WebApi.Controllers;
using Commands = SplurgeStop.Domain.PurchaseTransactionProfile.Commands;
using store = SplurgeStop.Domain.StoreProfile;

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

            var command = new Commands.CreateFull();
            command.Id = null;
            command.TransactionDate = DateTime.Now;

            // Add Store
            var store = await StoreHelpers.CreateValidStore();
            command.StoreId = store.Id;

            // Get Product
            var prod = await ProductHelpers.CreateValidProduct();

            // Add one LineItem
            command.LineItems = new List<LineItemStripped>
            {
                new LineItemStripped
                {
                    Price = price.ToString(CultureInfo.InvariantCulture),
                    CurrencyCode = "EUR",
                    CurrencySymbol = "€",
                    CurrencySymbolPosition = CurrencySymbolPosition.End,
                    Booking = Booking.Credit,
                    Notes = lineItem?.Notes,
                    Product = prod
                }
            };

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

            var command = new Commands.CreateFull
            {
                Id = null,
                TransactionDate = DateTime.Now
            };

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
                CurrencySymbolPosition = CurrencySymbolPosition.End,
                Notes = "New notes",
                Product = prod
            };

            command.LineItems = new List<LineItemStripped>
            {
                newLineItem
            };

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

            var updateCommand = new Commands.SetPurchaseTransactionDate
            {
                Id = id,
                TransactionDate = date
            };

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

            var updateCommand = new Commands.SetPurchaseTransactionLineItem
            {
                Id = id,
                LineItemId = lineItem.Id,
                Price = lineItem.Price.Amount,
                Currency = lineItem.Price.Currency.CurrencyCode,
                CurrencySymbol = lineItem.Price.Currency.CurrencySymbol,
                Booking = lineItem.Price.Booking,
                CurrencySymbolPosition = lineItem.Price.Currency.PositionRelativeToPrice,
                Notes = lineItem.Notes,
                Product = lineItem.Product
            };

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

            var updateCommand = new Commands.SetPurchaseTransactionStore
            {
                Id = id,
                StoreId = store.Id
            };

            await transactionController.Put(updateCommand);
        }

        public static async Task<PurchaseTransaction> ReloadPurchaseTransaction(PurchaseTransactionId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new PurchaseTransactionRepository(context);

            return await repository.GetAsync(id);
        }
    }
}
