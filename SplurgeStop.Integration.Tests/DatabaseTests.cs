using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.UI.WebApi.Controllers;
using Xunit;
using transaction = SplurgeStop.Domain.PurchaseTransaction;
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
            context.Database.ExecuteSqlRaw("DELETE FROM Purchases");
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
        public async Task Purchase_transaction_already_exists()
        {
            var repository = new PurchaseTransactionRepository(fixture.context);
            var unitOfWork = new EfCoreUnitOfWork(fixture.context);
            var service = new PurchaseTransactionService(repository, unitOfWork);
            var transaction = new transaction.PurchaseTransaction();
            transaction.SetTransactionDate(DateTime.Now);
            var transactionController = new PurchaseTransactionController(service);
            var command = new Commands.Create();
            command.Transaction = transaction;

            await transactionController.Post(command);

            await Assert.ThrowsAsync<InvalidOperationException>(async ()
                => await transactionController.Post(command));
        }

        [Fact]
        public async Task Purchase_transaction_inserted_to_database()
        {
            var repository = new PurchaseTransactionRepository(fixture.context);
            var unitOfWork = new EfCoreUnitOfWork(fixture.context);
            var service = new PurchaseTransactionService(repository, unitOfWork);
            var transaction = new transaction.PurchaseTransaction();
            transaction.SetTransactionDate(DateTime.Now);
            var command = new Commands.Create();
            command.Transaction = transaction;

            Assert.False(await repository.Exists(transaction.Id));

            var transactionController = new PurchaseTransactionController(service);
            var result = await transactionController.Post(command);
            Assert.True(await repository.Exists(transaction.Id));
            Assert.Equal(typeof(OkResult), result.GetType());
        }

        [Fact]
        public async Task Update_transaction_date()
        {
            PurchaseTransactionId transactionId = await CreateValidPurchaseTransaction();

            var repository = new PurchaseTransactionRepository(fixture.context);
            Assert.True(await repository.Exists(transactionId));

            var sut = await repository.Load(transactionId);
            Assert.Equal(DateTime.Now.Date, sut.PurchaseDate);

            await UpdatePurchaseDate(sut.Id, DateTime.Now.AddDays(-1));
            await fixture.context.Entry(sut).ReloadAsync();

            Assert.Equal(DateTime.Now.AddDays(-1).Date, sut.PurchaseDate);
        }

        //[Fact]
        //public async Task Update_transaction_date()
        //{
        //    var repository = new PurchaseTransactionRepository(fixture.context);
        //    var unitOfWork = new EfCoreUnitOfWork(fixture.context);
        //    var service = new PurchaseTransactionService(repository, unitOfWork);
        //    var transaction = new transaction.PurchaseTransaction();

        //    transaction.SetTransactionDate(DateTime.Now.AddDays(-1)); // TODO: change to Command!

        //    var command = new Commands.Create();
        //    command.Transaction = transaction;

        //    var transactionController = new PurchaseTransactionController(service);
        //    var result = await transactionController.Post(command);

        //    // TODO: Update Store
        //    // TODO: Update Line Items

        //    Assert.True(await repository.Exists(transaction.Id));
        //    Assert.Equal(typeof(OkResult), result.GetType());

        //    var sut = await repository.Load(transaction.Id);
        //    Assert.Equal(DateTime.Now.AddDays(-1).Date, sut.PurchaseDate);

        //    var updateCommand = new Commands.SetPurchaseTransactionDate();
        //    updateCommand.Id = sut.Id;
        //    updateCommand.TransactionDate = DateTime.Now;
        //    await transactionController.Put(updateCommand);

        //    sut = await repository.Load(transaction.Id);
        //    Assert.Equal(DateTime.Now.Date, sut.PurchaseDate);
        //}

        public void Only_Valid_Purchase_Transaction_Can_Be_Persisted_To_Database()
        {
            // Two first tests shouldn't execute Commit(), only Add()
        }
    }
}
