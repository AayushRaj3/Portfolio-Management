using AuthorizationApi1.Models;
using AuthorizationApi1.Provider;
using AuthorizationApi1.Repository;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace NUnitTestAuth
{
    public class Tests
    {
        private Mock<IConfiguration> _config;
        private IAuthenticationRepository _repo;
        private IAuthenticationProvider _provider;


        [SetUp]
        public void Setup()
        {
            _config = new Mock<IConfiguration>();
            _config.Setup(c => c["Jwt:key"]).Returns("Thisismysecretkey");
            _repo = new AuthenticationRepository(_config.Object);
            _provider = new AuthenticationProvider(_repo);

        }

        [Test]
        public void GetTokenPositiveTest()
        {
            User user = new User()
            {
                PortfolioId = 12345,
                Password = "abc@123"
            };
            var result = _provider.GetToken1(user);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetTokenNegativeTest()
        {
            User user = new User()
            {
                PortfolioId = 12345,
                Password = "yashi@123"
            };
            var result = _provider.GetToken1(user);
            Assert.IsNull(result);
        }
    }
}