using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.UI.WebApi.Controllers;
using Xunit;
using static SplurgeStop.Integration.Tests.HelperMethods;

namespace SplurgeStop.Integration.Tests
{
    public sealed class DatabaseFixture : IDisposable
    {
        internal SplurgeStopDbContext context;
        internal string connectionString;

        public DatabaseFixture()
        {
            connectionString = ConnectivityService.GetConnectionString("TEMP");
            context = new SplurgeStopDbContext(connectionString);
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            context.Database.ExecuteSqlRaw("DELETE FROM LineItem");
            context.Database.ExecuteSqlRaw("DELETE FROM Purchases");
            context.Database.ExecuteSqlRaw("DELETE FROM Stores");
        }
    }

    [Trait("Integration", "Database")]
    public sealed class DatabaseTests : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;

        public DatabaseTests(DatabaseFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact]
        public async Task Purchase_transaction_inserted_to_database()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(fixture.context);

            var repository = new PurchaseTransactionRepository(fixture.context);
            var sut = await repository.LoadPurchaseTransactionAsync(transactionId);
            
            await fixture.context.Entry(sut).ReloadAsync();

            Assert.True(await repository.ExistsAsync(transactionId));
        }

        [Fact]
        public async Task Purchase_transaction_already_exists()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(fixture.context);

            var repository = new PurchaseTransactionRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(transactionId));

            var unitOfWork = new EfCoreUnitOfWork(fixture.context);
            var service = new PurchaseTransactionService(repository, unitOfWork);
            var transaction = new PurchaseTransaction();
            
            var transactionController = new PurchaseTransactionController(service);
            var command = new Commands.Create();
            command.Transaction = transaction;

            await transactionController.Post(command);

            await Assert.ThrowsAsync<InvalidOperationException>(async ()
                => await transactionController.Post(command));
        }

        [Fact]
        public async Task Update_transaction_date()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(fixture.context);

            var repository = new PurchaseTransactionRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(transactionId));

            var sut = await repository.LoadFullPurchaseTransactionAsync(transactionId);

            Assert.Equal(DateTime.Now.Date, sut.PurchaseDate);

            await UpdatePurchaseDate(sut.Id, DateTime.Now.AddDays(-1), fixture.context);

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(DateTime.Now.AddDays(-1).Date, sut.PurchaseDate);
        }

        [Fact]
        public async Task Update_Store_name()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(fixture.context);

            var repository = new PurchaseTransactionRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(transactionId));

            var sut = await repository.LoadFullPurchaseTransactionAsync(transactionId);

            Assert.NotNull(sut.Store);
            Assert.Equal("Test market", sut.Store.Name);

            await UpdateStoreName(sut.Id, "Mega Market", fixture.context);

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal("Mega Market", sut.Store.Name);
        }

        [Fact]
        public async Task Add_transaction_lineItem()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(fixture.context);

            var repository = new PurchaseTransactionRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(transactionId));

            var sut = await repository.LoadFullPurchaseTransactionAsync(transactionId);

            Assert.Single(sut.LineItems);

            var lineItem = LineItemBuilder
                .LineItem(new Price(2.54m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end))
                .Build();

            await UpdateLineItems(sut.Id, lineItem, fixture.context);

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(2, sut.LineItems.Count);
        }

        [Fact]
        public async Task Total_price_of_line_items()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(fixture.context, 1.23m);

            var repository = new PurchaseTransactionRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(transactionId));

            var sut = await repository.LoadFullPurchaseTransactionAsync(transactionId);

            Assert.Single(sut.LineItems);

            var secondLineItem = LineItemBuilder
                .LineItem(new Price(2.54m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end))
                .Build();

            await UpdateLineItems(sut.Id, secondLineItem, fixture.context);

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(2, sut.LineItems.Count);
            var result = decimal.Parse(sut.TotalPrice.Substring(0, sut.TotalPrice.IndexOf(' ', StringComparison.Ordinal)));

            Assert.Equal(3.77m, result);
        }

        [Fact]
        public async Task Total_price_of_line_items_with_debit_item()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(fixture.context, 1.23m);

            var repository = new PurchaseTransactionRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(transactionId));

            var sut = await repository.LoadFullPurchaseTransactionAsync(transactionId);

            Assert.Single(sut.LineItems);

            var secondLineItem = LineItemBuilder.LineItem(new Price(2.54m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end))
                .Build();
            await UpdateLineItems(sut.Id, secondLineItem, fixture.context);

            var debitLineItem = LineItemBuilder.LineItem(new Price(1.54m, Booking.Debit, "EUR", "€", CurrencySymbolPosition.end))
                .Build();
            await UpdateLineItems(sut.Id, debitLineItem, fixture.context);

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(3, sut.LineItems.Count);
            Assert.Equal(2, sut.LineItems.Count(i => i.Price.Booking == Booking.Credit));
            Assert.Equal(1, sut.LineItems.Count(i => i.Price.Booking == Booking.Debit));

            var result = decimal.Parse(sut.TotalPrice.Substring(0, sut.TotalPrice.IndexOf(' ', StringComparison.Ordinal)));

            Assert.Equal(2.23m, result);
        }

        [Fact]
        public async Task Add_lineItem_with_notes() 
        {
            var lineItem = LineItemBuilder.LineItem(new Price(1.00m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end))
                .WithNotes("My Notes!")
                .Build();
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(fixture.context,1m, lineItem);

            var repository = new PurchaseTransactionRepository(fixture.context);
            var sut = await repository.LoadFullPurchaseTransactionAsync(transactionId);

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.True(await repository.ExistsAsync(transactionId));
            Assert.Equal("My Notes!", sut.LineItems.FirstOrDefault().Notes);
        }

        [Fact]
        public async Task Update_LineItem_price()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(fixture.context, 22.33m);

            var repository = new PurchaseTransactionRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(transactionId));

            var sut = await repository.LoadFullPurchaseTransactionAsync(transactionId);

            Assert.NotNull(sut.LineItems);
            Assert.Single(sut.LineItems);
            Assert.Equal(22.33m, sut.LineItems.FirstOrDefault().Price.Amount);

            var updatedLineItem = sut.LineItems.FirstOrDefault();
            
            updatedLineItem.UpdateLineItemPrice(new Price(33.44m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end));

            await UpdateLineItems(sut.Id, updatedLineItem, fixture.context);

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Single(sut.LineItems);
            Assert.Equal(33.44m, sut.LineItems.FirstOrDefault().Price.Amount);
        }

        [Fact]
        public async Task Invalid_Purchase_Transaction_Cannot_Be_Persisted_To_Database()
        {
            var repository = new PurchaseTransactionRepository(fixture.context);
            var unitOfWork = new EfCoreUnitOfWork(fixture.context);
            var service = new PurchaseTransactionService(repository, unitOfWork);
            var transaction = new PurchaseTransaction();
         
            var transactionController = new PurchaseTransactionController(service);
            var command = new Commands.Create();
            command.Transaction = transaction;

            await transactionController.Post(command);
            var sut = await fixture.context.Entry(transaction).GetDatabaseValuesAsync();

            Assert.Null(sut);
        }
    }
}
