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
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AuthController));
        private readonly IAuthenticationProvider _provider;

        public AuthController(IAuthenticationProvider provider)
        {
            _log4net.Info("AuthController constructor initiated.");
            _provider = provider;
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            _log4net.Info("AuthController Login method initiated.");
            try
            {
                IActionResult response = Unauthorized();
                var token = _provider.Login(user);

                if (token != null)
                {
                    _log4net.Info("Token received.");
                    response = Ok(new { tokenString = token });
                }
                _log4net.Info("Response is given user is authorized or unauthorized.");
                return response;
                
            }
            catch(Exception exception)
            {
                _log4net.Info("Exception found in AuthController Login method="+exception.Message);
                return BadRequest("Some Internal Server Error Occurred!!");
            }
        }
        

        


    }
}
