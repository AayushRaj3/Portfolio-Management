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
        private readonly IAuthenticationRepository _repository;

        public AuthenticationProvider(IAuthenticationRepository repository)
        {
            this._repository = repository;
        }
        public string GetToken(User user)
        {
            var token = _repository.GenerateToken(user);
            if (token == null)
                return null;
            return token;
        }
    }
}
