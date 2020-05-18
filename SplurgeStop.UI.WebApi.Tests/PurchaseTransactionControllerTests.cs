using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuidHelpers;
using Moq;
using SplurgeStop.Domain;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.PurchaseTransaction.DTO;
using SplurgeStop.UI.WebApi.Controllers;
using Xunit;

namespace SplurgeStop.UI.WebApi.Tests
{
    public sealed class PurchaseTransactionControllerTests
    {
        [Fact]
        public async Task Get_All_PurchaseTransactions()
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
                    TotalPrice = (decimal)12.34 * i,
                });
            }

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
    }
}
