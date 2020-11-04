using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationApi1.Models;
using AuthorizationApi1.Provider;
using AuthorizationApi1.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationApi1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AuthController));
        private readonly IAuthenticationProvider provider;

        public AuthController(IAuthenticationProvider provider)
        {
            this.provider = provider;
        }

        [AllowAnonymous]
        [HttpPost("GetToken")]
        public IActionResult GetToken(User user)
        {
            _log4net.Info("Token review");
            var token = provider.GetToken1(user);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }
    }
}
