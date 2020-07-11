using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuidHelpers;
using Moq;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.PurchaseTransactionProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.LineItemProfile;
using SplurgeStop.Domain.Shared;
using SplurgeStop.Domain.StoreProfile;
using SplurgeStop.UI.WebApi.Controllers;
using Xunit;

namespace SplurgeStop.UI.WebApi.Tests
{
    [Trait("Unit tests", "WebApi/PurchaseTransactionController")]
    public sealed class PurchaseTransactionControllerTests
    {
        private static List<PurchaseTransactionStripped> MockPurchaseTransactions()
        {
            var mockPurchaseTransactions = new List<PurchaseTransactionStripped>();

            for (int i = 1; i <= 10; i++)
            {
                mockPurchaseTransactions.Add(new PurchaseTransactionStripped
                {
                    Id = SequentialGuid.NewSequentialGuid(),
                    ItemCount = i,
                    PurchaseDate = DateTime.Now.AddDays(-i),
                    StoreName = $"Store-{i}",
                    TotalPrice = $"{(decimal)12.34 * i} €",
                });
            }

            return mockPurchaseTransactions;
        }

        [Fact]
        public async Task Get_All_PurchaseTransactions()
        {
            List<PurchaseTransactionStripped> mockPurchaseTransactions = MockPurchaseTransactions();

            var mockRepository = new Mock<IPurchaseTransactionRepository>();
            mockRepository.Setup(repo => repo.GetAllDtoAsync())
                .Returns(() => Task.FromResult(mockPurchaseTransactions.AsEnumerable()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var purchaseTransactionService = new PurchaseTransactionService(mockRepository.Object, mockUnitOfWork.Object);

            var purchaseTransactionController = new PurchaseTransactionController(purchaseTransactionService);
            var result = await purchaseTransactionController.GetPurchaseTransactions();

            Assert.Equal(10, result.Count());
            mockRepository.Verify(mock => mock.GetAllDtoAsync(), Times.Once());
        }


        [Fact]
        public async Task Valid_Id_Returns_PurchaseTransaction()
        {
            var id = Guid.NewGuid();
            var store = Store.Create(new StoreId(SequentialGuid.NewSequentialGuid()), "Kwik-E-Mart");
            var lineItem = new List<LineItemStripped>();
            PurchaseDate transactionDate = new PurchaseDate(DateTime.Now);
            var purchaseTransaction = PurchaseTransaction.CreateFull(id, store, lineItem, transactionDate);

            var mockPurchaseTransactionService = new Mock<IPurchaseTransactionService>();
            mockPurchaseTransactionService.Setup(s => s.GetDetailedPurchaseTransaction(id))
                .Returns(() => Task.FromResult(purchaseTransaction));

            var purchaseTransactionController = new PurchaseTransactionController(mockPurchaseTransactionService.Object);
            var result = await purchaseTransactionController.GetPurchaseTransaction(id);

            mockPurchaseTransactionService.Verify(mock => mock.GetDetailedPurchaseTransaction(id), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Invalid_Id_Returns_Null()
        {
            var mockPurchaseTransaction = new Mock<PurchaseTransaction>();
            var id = Guid.NewGuid();

            var mockPurchaseTransactionService = new Mock<IPurchaseTransactionService>();
            mockPurchaseTransactionService.Setup(s => s.GetDetailedPurchaseTransaction(Guid.NewGuid()))
                .Returns(() => Task.FromResult(mockPurchaseTransaction.Object));

            var purchaseTransactionController = new PurchaseTransactionController(mockPurchaseTransactionService.Object);

            var result = await purchaseTransactionController.GetPurchaseTransaction(id);
            Assert.Null(result);
        }
    }
}
