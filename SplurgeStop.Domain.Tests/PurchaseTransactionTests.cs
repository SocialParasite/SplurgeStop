﻿using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.LineItemProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.PriceProfile;
using SplurgeStop.Domain.StoreProfile;
using Xunit;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Domain/PurchaseTransaction")]
    public sealed class PurchaseTransactionTests
    {
        [Fact]
        public void PurchaseTransaction_created()
        {
            var purchaseTransactionId = new PurchaseTransactionId(SequentialGuid.NewSequentialGuid());
            var storeId = new StoreId(SequentialGuid.NewSequentialGuid());
            var store = Store.Create(storeId, "Kwik-E-Mart");
            var sut = PurchaseTransaction.CreateFull(purchaseTransactionId, store, new List<LineItemStripped>(), DateTime.Now);

            Assert.NotNull(sut);
            Assert.Contains("Kwik-E-Mart", sut.Store.Name);
            Assert.NotNull(sut.LineItems);
        }

        public static List<object[]> InvalidPurchaseTransactionData = new List<object[]>
        {
            new object[]
            {
                null,
                Store.Create(new StoreId(SequentialGuid.NewSequentialGuid()), "Kwik-E-Mart"),
                new List<LineItemStripped>()
            },
            new object[]
                {
                    new PurchaseTransactionId(SequentialGuid.NewSequentialGuid()),
                    Store.Create(new StoreId(SequentialGuid.NewSequentialGuid()), "Kwik-E-Mart"),
                    null
                },
            new object[]
            {
                new PurchaseTransactionId(SequentialGuid.NewSequentialGuid()),
                null,
                new List<LineItemStripped>()
            },
            new object[] { null, null, null },
        };

        private static PurchaseTransaction CreatePurchaseTransaction()
        {
            var purchaseTransactionId = new PurchaseTransactionId(SequentialGuid.NewSequentialGuid());
            var storeId = new StoreId(SequentialGuid.NewSequentialGuid());
            var store = Store.Create(storeId, "Kwik-E-Mart");
            return PurchaseTransaction
                .CreateFull(purchaseTransactionId, store, new List<LineItemStripped>(), DateTime.Now);
        }

        [Theory]
        [MemberData(nameof(InvalidPurchaseTransactionData))]
        public void PurchaseTransaction_not_created(PurchaseTransactionId purchaseTransactionId, Store store, ICollection<LineItemStripped> lineItems)
        {
            Action sut = () => PurchaseTransaction.CreateFull(purchaseTransactionId, store, lineItems, DateTime.Now);

            Assert.Throws<ArgumentNullException>(sut.Invoke);
        }

        [Fact]
        public void Valid_store_update()
        {
            var sut = CreatePurchaseTransaction();

            var newStore = Store.Create(new StoreId(SequentialGuid.NewSequentialGuid()), "City-wok");

            sut.UpdateStore(newStore);

            Assert.Equal(newStore.Id, sut.Store.Id);
        }

        [Fact]
        public void Invalid_store_update()
        {
            var sut = CreatePurchaseTransaction();
            Assert.Throws<ArgumentNullException>(() => sut.UpdateStore(null));
        }

        [Fact]
        public void Valid_purchase_date_update()
        {
            var sut = CreatePurchaseTransaction();
            Assert.Equal(DateTime.Now.Day, sut.PurchaseDate.Value.Day);

            var newDate = DateTime.Now.AddDays(-1);
            sut.UpdatePurchaseTransactionDate(newDate);

            Assert.Equal(DateTime.Now.AddDays(-1).Day, sut.PurchaseDate.Value.Day);
        }

        [Fact]
        public void Valid_line_item_update()
        {
            var sut = CreatePurchaseTransaction();
            Assert.NotNull(sut.LineItems);
            Assert.True(sut.LineItems.Count == 0);

            var brand = Brand.Create(Guid.NewGuid(), "brand");
            var productType = ProductType.Create(Guid.NewGuid(), "product type");
            var size = Size.Create(Guid.NewGuid(), "your size");

            var product = Product.Create(Guid.NewGuid(), "prod", brand, productType, size);

            var newLineItem = LineItemBuilder
                .LineItem(new Price(1.1m))
                .WithProduct(product)
                .Build();
            sut.UpdateLineItem(newLineItem);

            Assert.True(sut.LineItems.Count == 1);
        }

        [Fact]
        public void Total_sum_of_line_items()
        {
            var sut = CreatePurchaseTransaction();

            var brand = Brand.Create(Guid.NewGuid(), "brand");
            var productType = ProductType.Create(Guid.NewGuid(), "product type");
            var size = Size.Create(Guid.NewGuid(), "your size");

            var product = Product.Create(Guid.NewGuid(), "prod", brand, productType, size);
            for (int i = 1; i < 6; i++)
            {
                var lineItem = LineItemBuilder.LineItem(new Price(1.0m * i))
                    .WithProduct(product)
                    .Build();
                sut.UpdateLineItem(lineItem);
            }

            Assert.Contains("15.0", sut.TotalPrice);
        }

        [Fact]
        public void Total_sum_with_no_line_items()
        {
            var sut = CreatePurchaseTransaction();

            Assert.Equal("N/A", sut.TotalPrice);
        }
    }
}
