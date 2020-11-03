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
        private readonly IMutualProvider pro;
        public MutualFundNAVController(IMutualProvider pro)
        {
            this.pro = pro;
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            if (name == null)
            {
                return BadRequest();
            }
            try
            {
                var data = pro.GetDailyNAV(name);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}