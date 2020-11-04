using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthorizationApi1.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationApi1.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly Dictionary<int, string> users = new Dictionary<int, string>() {
            { 12345,"abc@123"},
            {67890,"test@123" }
        };
        private readonly IConfiguration config;

     
        public AuthenticationRepository(IConfiguration config)
        {
            this.config = config;
        }

        public string GenerateToken(User user)
        {
            if (!users.Any(u => u.Key == user.PortfolioId && u.Value == user.Password))
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(config["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, (user.PortfolioId).ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
