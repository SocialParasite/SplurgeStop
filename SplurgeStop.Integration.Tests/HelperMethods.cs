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
using city = SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.CityProfile;

namespace SplurgeStop.Integration.Tests
{
    public static class HelperMethods
    {
        public async static Task<PurchaseTransactionId> CreateValidPurchaseTransaction(decimal price = 1.00m,
                                                                                       LineItem lineItem = null,
                                                                                       string product = "")
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
            var store = await CreateValidStore();
            command.StoreId = store.Id;

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
                Product = product
            });

            // Create PurchaseTransaction
            var transactionController = new PurchaseTransactionController(service);
            await transactionController.Post(command);

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


        public async static Task<City> CreateValidCity()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CityRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CityService(repository, unitOfWork);

            var command = new city.Commands.Create();
            command.Name = "New Mansester";
            command.Id = null;

            var cityController = new CityController(service);
            var city = await cityController.Post(command);

            return await repository.GetCityAsync(city.Value.Id);
        }

        public async static Task<dynamic> CreateInvalidCity()
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CityRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CityService(repository, unitOfWork);

            var command = new city.Commands.Create();
            command.Id = null;

            // Create Store
            var cityController = new CityController(service);
            return await cityController.Post(command);
        }

        public async static Task UpdateCityName(CityId id, string name)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CityRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CityService(repository, unitOfWork);
            var cityController = new CityController(service);

            var updateCommand = new city.Commands.SetCityName();
            updateCommand.Id = id;
            updateCommand.Name = name;

            await cityController.Put(updateCommand);
        }

        public async static Task<bool> CheckIfCityExists(CityId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CityRepository(context);

            return await repository.ExistsAsync(id);
        }

        public async static Task RemoveCity(CityId id)
        {
            var connectionString = ConnectivityService.GetConnectionString("TEMP");
            var context = new SplurgeStopDbContext(connectionString);
            var repository = new CityRepository(context);
            var unitOfWork = new EfCoreUnitOfWork(context);
            var service = new CityService(repository, unitOfWork);
            var cityController = new CityController(service);

            var updateCommand = new city.Commands.DeleteCity();
            updateCommand.Id = id;

            await cityController.DeleteCity(updateCommand);
        }

    }
}
