using System;
using Xunit;
using transaction = SplurgeStop.Domain.PurchaseTransaction;

namespace SplurgeStop.Domain.Tests
{
    [Trait("Unit tests", "Purchase transaction")]
    public class PurchaseTransactionTests
    {
        [Fact]
        public void Invalid_Purchase_Date()
        {
            var sut = new transaction.PurchaseTransaction();
            
            Assert.Throws<ArgumentException>(() => sut.SetTransactionDate(default));
        }

        [Fact]
        public void Valid_Purchase_Date()
        {
            var sut = new transaction.PurchaseTransaction();
            sut.SetTransactionDate(DateTime.Now);
           
            Assert.Equal(DateTime.Now.Date, sut.PurchaseDate);
        }
    }
}
