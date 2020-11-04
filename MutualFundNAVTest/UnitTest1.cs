using NUnit.Framework;
using Moq;
using Castle.Core.Configuration;
using DailyMutualFundNAVMicroservice.Models;
using DailyMutualFundNAVMicroservice.Repository;
using DailyMutualFundNAVMicroservice.Provider;
using DailyMutualFundNAVMicroservice.Controllers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace MutualFundNAVTest
{
    public class Tests
    {
       // private Mock<IMutualFund> _repo;
        private Mock<IMutualProvider> _pro;
        public IMutualProvider pro;
        [SetUp]
        public void Setup()
        {
            //_repo = new Mock<IMutualFund>();
            _pro = new Mock<IMutualProvider>();
            var listoffunds = new List<MutualFundDetails>();
            listoffunds.Add(new MutualFundDetails
            {
                MutualFundId = 1234,
                MutualFundName = "Dummy",
                MutualFundValue = 123.0
            });
            _pro.Setup(m => m.GetDailyNAV(
                It.IsAny<string>())).Returns((string s) => listoffunds.FirstOrDefault(
                x => x.MutualFundName == s));
                pro = _pro.Object;
        }

        [Test]
        public void GetMutualFundDetailTest()
        {
            MutualFundDetails result = pro.GetDailyNAV("Dummy");
            Assert.IsNotNull(result);
        }
        [Test]
        public void GetMutualFundDetailWithoutInput()
        {
            MutualFundDetails result = pro.GetDailyNAV("ABCD");
            Assert.IsNull(result);
        }
    }
}