using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationApi1.Models;
using AuthorizationApi1.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationApi1.Provider
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly IAuthenticationRepository repo;

        public AuthenticationProvider(IAuthenticationRepository repo)
        {
            this.repo = repo;
        }
        public string GetToken1(User user)
        {
            var token = repo.GenerateToken(user);
            if (token == null)
                return null;
            return token;

        }
    }
}
