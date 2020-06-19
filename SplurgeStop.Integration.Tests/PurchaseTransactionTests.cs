﻿using System;
using System.Linq;
using System.Threading.Tasks;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain.PurchaseTransaction;
using Xunit;
using static SplurgeStop.Integration.Tests.HelperMethods;

namespace SplurgeStop.Integration.Tests
{
    [Trait("Integration", "Database")]
    public sealed partial class DatabaseTests
    {
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
        public async Task FullPurchase_transaction_inserted_to_database()
        {
            PurchaseTransactionId transactionId = await CreateFullValidPurchaseTransaction();

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
    }
}