using Castle.Core.Configuration;
using DailySharePriceApi.Controllers;
using DailySharePriceApi.Models;
using DailySharePriceApi.Provider;
using DailySharePriceApi.Repository;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace StockApiTesting
{
    public class Tests
    {

        List<Stock> stocks = new List<Stock>();
        public IStockProvider stockpro;

        [SetUp]
        public void Setup()
        {
            stocks = new List<Stock>()
            {
                new Stock{ StockId=1, StockName="Dummy1", StockValue=1},
                new Stock{ StockId=2, StockName="Dummy2", StockValue=2}
            };

            Mock<IStockProvider> mockpro = new Mock<IStockProvider>();

            mockpro.Setup(m => m.GetStockByName(
                It.IsAny<string>())).Returns((string s) => stocks.FirstOrDefault(
                x => x.StockName == s));

            stockpro = mockpro.Object;
        }    

        [Test]
        public void GetStockByName_PassCase()
        {
            Stock result = stockpro.GetStockByName("Dummy2");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetStockByName_FailCase()
        {
            Stock result = stockpro.GetStockByName("ABC");
            Assert.IsNull(result);
        }
    }
}