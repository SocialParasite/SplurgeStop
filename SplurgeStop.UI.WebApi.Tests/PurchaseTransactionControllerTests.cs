using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuidHelpers;
using Moq;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.Shared;
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
            mockRepository.Setup(repo => repo.GetAllPurchaseTransactionsAsync())
                .Returns(() => Task.FromResult(mockPurchaseTransactions.AsEnumerable()));

            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var purchaseTransactionService = new PurchaseTransactionService(mockRepository.Object, mockUnitOfWork.Object);

            var purchaseTransactionController = new PurchaseTransactionController(purchaseTransactionService);
            var result = await purchaseTransactionController.GetPurchaseTransactions();

            Assert.Equal(10, result.Count());
            mockRepository.Verify(mock => mock.GetAllPurchaseTransactionsAsync(), Times.Once());
        }


        [Fact]
        public async Task Valid_Id_Returns_PurchaseTransaction()
        {
            var mockRepository = new Mock<IPurchaseTransactionRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockPurchaseTransaction = new Mock<PurchaseTransaction>();
            var id = Guid.NewGuid();

            var mockPurchaseTransactionService = new Mock<IPurchaseTransactionService>();
            mockPurchaseTransactionService.Setup(s => s.GetDetailedPurchaseTransaction(id))
                .Returns(() => Task.FromResult(mockPurchaseTransaction.Object));

            var purchaseTransactionController = new PurchaseTransactionController(mockPurchaseTransactionService.Object);
            var result = await purchaseTransactionController.GetPurchaseTransaction(id);

            mockPurchaseTransactionService.Verify(mock => mock.GetDetailedPurchaseTransaction(id), Times.Once());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Invalid_Id_Returns_Null()
        {
            var mockRepository = new Mock<IPurchaseTransactionRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
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
