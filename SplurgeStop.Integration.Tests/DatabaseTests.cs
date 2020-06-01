using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.StoreProfile;
using Xunit;
using static SplurgeStop.Integration.Tests.HelperMethods;
using store = SplurgeStop.Domain.StoreProfile;

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


        // PURCHASETRANSACTION
        [Fact]
        public async Task Purchase_transaction_inserted_to_database()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction();

            var repository = new PurchaseTransactionRepository(fixture.context);
            var sut = await repository.GetPurchaseTransactionFullAsync(transactionId);

            Assert.True(await repository.ExistsAsync(transactionId));
            Assert.Equal(DateTime.Now.Date, sut.PurchaseDate);
            Assert.NotNull(sut.Store);
            Assert.Single(sut.LineItems);
        }

        [Fact]
        public async Task Update_transaction_date()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction();

            var repository = new PurchaseTransactionRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(transactionId));

            var sut = await repository.LoadFullPurchaseTransactionAsync(transactionId);

            Assert.Equal(DateTime.Now.Date, sut.PurchaseDate);

            await UpdatePurchaseDate(sut.Id, DateTime.Now.AddDays(-1));

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(DateTime.Now.AddDays(-1).Date, sut.PurchaseDate);
        }
        
        // PURCHASETRANSACTION
        // LineItems
        [Fact]
        public async Task Add_transaction_lineItem()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction();

            var repository = new PurchaseTransactionRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(transactionId));

            var sut = await repository.GetPurchaseTransactionFullAsync(transactionId);

            Assert.Single(sut.LineItems);

            var lineItem = LineItemBuilder
                .LineItem(new Price(2.54m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end))
                .Build();

            await UpdateLineItem(sut.Id, lineItem);

            sut = await ReloadPurchaseTransaction(sut.Id);

            Assert.Equal(2, sut.LineItems.Count);
        }

        [Fact]
        public async Task Total_price_of_line_items()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(1.23m);

            var repository = new PurchaseTransactionRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(transactionId));

            var sut = await repository.GetPurchaseTransactionFullAsync(transactionId);

            Assert.Single(sut.LineItems);

            var secondLineItem = LineItemBuilder
                .LineItem(new Price(2.54m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end))
                .Build();

            await UpdateLineItem(sut.Id, secondLineItem);

            sut = await ReloadPurchaseTransaction(sut.Id);

            Assert.Equal(2, sut.LineItems.Count);
            var result = decimal.Parse(sut.TotalPrice.Substring(0, sut.TotalPrice.IndexOf(' ', StringComparison.Ordinal)));

            Assert.Equal(3.77m, result);
        }

        [Fact]
        public async Task Total_price_of_line_items_with_debit_item()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(1.23m);

            var repository = new PurchaseTransactionRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(transactionId));

            var sut = await repository.GetPurchaseTransactionFullAsync(transactionId);

            Assert.Single(sut.LineItems);

            var secondLineItem = LineItemBuilder.LineItem(new Price(2.54m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end))
                .Build();
            await UpdateLineItem(sut.Id, secondLineItem);

            var debitLineItem = LineItemBuilder.LineItem(new Price(1.54m, Booking.Debit, "EUR", "€", CurrencySymbolPosition.end))
                .Build();
            await UpdateLineItem(sut.Id, debitLineItem);

            sut = await ReloadPurchaseTransaction(sut.Id);

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
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(1m, lineItem);

            var repository = new PurchaseTransactionRepository(fixture.context);
            var sut = await repository.GetPurchaseTransactionFullAsync(transactionId);

            Assert.True(await repository.ExistsAsync(transactionId));
            Assert.Equal("My Notes!", sut.LineItems.FirstOrDefault().Notes);
        }

        [Fact]
        public async Task Update_LineItem_price()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction(22.33m);

            var repository = new PurchaseTransactionRepository(fixture.context);
            
            var sut = await repository.GetPurchaseTransactionFullAsync(transactionId);

            Assert.NotNull(sut.LineItems);
            Assert.Single(sut.LineItems);
            Assert.Equal(22.33m, sut.LineItems.FirstOrDefault().Price.Amount);

            var lineItemId = sut.LineItems.FirstOrDefault().Id;

            var updatedLineItem = LineItemBuilder
                .LineItem(new Price(33.44m, Booking.Credit, "EUR", "€", CurrencySymbolPosition.end), lineItemId)
                .Build();

            await UpdateLineItem(transactionId, updatedLineItem);

            sut = await ReloadPurchaseTransaction(sut.Id);

            Assert.Single(sut.LineItems);
            Assert.Equal(33.44m, sut.LineItems.FirstOrDefault().Price.Amount);
        }

        // STORE
        [Fact]
        public async Task Store_inserted_to_database()
        {
            Store store = await CreateValidStore();

            var repository = new StoreRepository(fixture.context);
            var sut = await repository.LoadStoreAsync(store.Id);

            Assert.True(await repository.ExistsAsync(sut.Id));
            Assert.True(sut.Name.Length > 0);
        }

        [Fact]
        public async Task Invalid_Store()
        {
            Store store = await CreateInvalidStore();

            Assert.Null(store);
        }

        [Fact]
        public async Task Update_Store_name()
        {
            Store store = await CreateValidStore();

            var repository = new StoreRepository(fixture.context);
            Assert.True(await repository.ExistsAsync(store.Id));

            var sut = await repository.LoadFullStoreAsync(store.Id);

            var storeId = sut.Id;

            Assert.NotNull(sut);
            Assert.Equal("Test market", sut.Name);

            await UpdateStoreName(sut.Id, "Mega Market");

            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal("Mega Market", sut.Name);
            Assert.Equal(storeId, sut.Id);
        }
    }
}
