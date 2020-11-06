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
        private readonly Dictionary<int, string> users = new Dictionary<int, string>()
        {
            { 12345,"admin1" },
            { 67890,"admin2" }
        };
        
        private readonly IConfiguration _config;

     
        public AuthenticationRepository(IConfiguration config)
        {
            _config = config;
        }

       public string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Sub, user.PortfolioId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.PortfolioId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User AuthenticateUser(User user)
        {

            if (users.Any(u =>u.Key == user.PortfolioId && u.Value == user.Password))
            {
                return user;
            }
            return null;
        }

        public string Login(User login)
        {
            //IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                return tokenString;
            }

            return null;
        }
    }
}
