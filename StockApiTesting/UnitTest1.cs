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
        public IStockRepository stockrepo;

        [SetUp]
        public void Setup()
        {
            stocks = new List<Stock>()
            {
                new Stock{ StockId=1, StockName="Dummy1", StockValue=1},
                new Stock{ StockId=2, StockName="Dummy2", StockValue=2}
            };

            Mock<IStockProvider> mockpro = new Mock<IStockProvider>();
            mockpro.Setup(m => m.GetStockByNameProvider(
                It.IsAny<string>())).Returns((string s) => stocks.FirstOrDefault(
                x => x.StockName == s));
            stockpro = mockpro.Object;

            Mock<IStockRepository> mockrepo = new Mock<IStockRepository>();
            mockrepo.Setup(m => m.GetStockByNameRepository(
                It.IsAny<string>())).Returns((string s) => stocks.FirstOrDefault(
                x => x.StockName == s));

            stockrepo = mockrepo.Object;
        }

        [Test]
        public void GetStockByNameProvider_PassCase()
        {
            Stock result = stockpro.GetStockByNameProvider("Dummy2");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetStockByNameProvider_FailCase()
        {
            Stock result = stockpro.GetStockByNameProvider("ABC");
            Assert.IsNull(result);
        }

        [Test]
        public void GetStockByNameRepository_PassCase()
        {
            Stock result = stockrepo.GetStockByNameRepository("Dummy1");
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetStockByNameRepository_FailCase()
        {
            Stock result = stockrepo.GetStockByNameRepository("KBC");
            Assert.IsNull(result);
        }
    }
}