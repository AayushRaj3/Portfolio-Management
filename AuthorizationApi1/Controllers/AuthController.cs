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
        private readonly IAuthenticationProvider _provider;

        public AuthController(IAuthenticationProvider provider)
        {
            this._provider = provider;
        }

        [AllowAnonymous]
        [HttpPost("AutheticateUser")]
        public IActionResult AuthenticateUser(User user)
        {
            try
            {
                _log4net.Info("AuthenticateUser Initiated");
                var token = _provider.GetToken(user);
                if (token == null)
                {
                    _log4net.Info("Not an authenticated user");
                    return Unauthorized();
                }
                    _log4net.Info("Authenticated user");
                    return Ok(token);
            }
            catch (Exception exception) 
            {
                _log4net.Info("Exception found!");
                return BadRequest(exception.Message);
            }
        }
    }
}
