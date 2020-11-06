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
        private Mock<IConfiguration> _mockconfig;
        private IAuthenticationRepository _repository;
        private IAuthenticationProvider _provider;


        [SetUp]
        public void Setup()
        {
            _mockconfig = new Mock<IConfiguration>();
            _mockconfig.Setup(c => c["Jwt:key"]).Returns("Thisismysecretkey");
            _repository = new AuthenticationRepository(_mockconfig.Object);
            _provider = new AuthenticationProvider(_repository);


        }

        [Test]
        public void GetTokenPositiveTest()
        {
            User user = new User()
            {
                PortfolioId = 12345,
                Password = "abc@123"
            };
            var result = _provider.GetToken(user);
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
            var result = _provider.GetToken(user);
            Assert.IsNull(result);
        }

        [Test]
        public void GenerateTokenPositiveTest()
        {
            User user = new User()
            {
                PortfolioId = 12345,
                Password = "abc@123"
            };
            var result = _repository.GenerateToken(user);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GenerateTokenNegativeTest()
        {
            User user = new User()
            {
                PortfolioId = 12345,
                Password = "yashi@123"
            };
            var result = _repository.GenerateToken(user);
            Assert.IsNull(result);
        }
    }
}