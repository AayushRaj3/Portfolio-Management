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
        private Mock<IMutualFund> _repo;
        private Mock<IMutualProvider> _pro;
        public IMutualProvider pro;
        public IMutualFund repo;
        [SetUp]
        public void Setup()
        {
            _repo = new Mock<IMutualFund>();
            _pro = new Mock<IMutualProvider>();
            var listoffunds = new List<MutualFundDetails>();
            listoffunds.Add(new MutualFundDetails
            {
                MutualFundId = 1234,
                MutualFundName = "Dummy",
                MutualFundValue = 123.0
            });
            listoffunds.Add(new MutualFundDetails
            {
                MutualFundId = 124,
                MutualFundName = "Dummy1",
                MutualFundValue = 223.5
            });
            _pro.Setup(m => m.GetMutualFundByNamePro(
                It.IsAny<string>())).Returns((string s) => listoffunds.FirstOrDefault(
                x => x.MutualFundName == s));
                pro = _pro.Object;

            _repo.Setup(m => m.GetMutualFundByNameRepo(
                It.IsAny<string>())).Returns((string s) => listoffunds.FirstOrDefault(
                x => x.MutualFundName == s));
             repo = _repo.Object;
        }

        [Test]
        public void GetMutualFundByName_PassCase_Provider()
        {
            MutualFundDetails result = pro.GetMutualFundByNamePro("Dummy");
            Assert.IsNotNull(result);
        }
        [Test]
        public void GetMutualFundByName_FailCase_Provider()
        {
            MutualFundDetails result = pro.GetMutualFundByNamePro("ABCD");
            Assert.IsNull(result);
        }
        [Test]
        public void GetMutualFundByName_PassCase_Repository()
        {
            MutualFundDetails result = repo.GetMutualFundByNameRepo("Dummy");
            Assert.IsNotNull(result);
        }
        [Test]
        public void GetMutualFundByName_FailCase_Repository()
        {
            MutualFundDetails result = repo.GetMutualFundByNameRepo("ABCD");
            Assert.IsNull(result);
        }
    }
}