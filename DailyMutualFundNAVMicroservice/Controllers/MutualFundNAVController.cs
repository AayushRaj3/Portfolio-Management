using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DailyMutualFundNAVMicroservice.Repository;
using DailyMutualFundNAVMicroservice.Models;
using DailyMutualFundNAVMicroservice.Provider;

namespace DailyMutualFundNAVMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MutualFundNAVController : ControllerBase
    {
        static readonly log4net.ILog _log4net=log4net.LogManager.GetLogger(typeof(MutualFundNAVController));
        private readonly IMutualFundProvider pro;
        public MutualFundNAVController(IMutualFundProvider _pro)
        {
            pro = _pro;
        }
        [HttpGet("{name}")]
        public IActionResult GetMutualFundDetailsByName(string name)
        {
            if (name == null)
            {
                _log4net.Info("MutualFundNAVController Null Name");
                return BadRequest("Name Cannot be null");
            }
            _log4net.Info("MutualFundController HttpGet GetMutualFundDetailsByName and " + name + " is searched");
            try
            { 
                var data = pro.GetMutualFundByNamePro(name);
                if (data == null)
                {
                    _log4net.Info("MutualFundNAVController Invalid MutualFund Name ");
                    return NotFound("Invalid MutualFund Name");
                }
                else
                {
                    _log4net.Info("MutualFundNAVController MutualFund Found");
                    return Ok(data);
                }
                
            }
            catch (Exception ex)
            {
                _log4net.Info("MutualFundNAVController Exception Found=" + ex.Message) ;
                return BadRequest("Internal Server Error");   
            }
        }
    }
}