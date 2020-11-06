using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailySharePriceApi.Models;
using DailySharePriceApi.Provider;
using DailySharePriceApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailySharePriceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(StockController));

        private readonly IStockProvider _provider;

        public StockController(IStockProvider provider)
        {
            _provider = provider;
        }

        [HttpGet("{name}")]
        public IActionResult GetStockByName(string name)
        {
            _log4net.Info("StockController HttpGet GetStockByName and " + name + " is searched");
            try
            {
                if (name == null)
                {
                    _log4net.Info("StockController Null Name");
                    return BadRequest();
                }
                else
                {
                    var result = _provider.GetStockByNameProvider(name.ToUpper());
                    if (result == null)
                    {
                        _log4net.Info("StockController Stock " + name + "Not Found");
                        return NotFound("Stock Not Found");
                    }
                    else
                    {
                        _log4net.Info("StockController Stock Found");
                        return Ok(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _log4net.Info("Stock Controller Exception Found - " + ex.Message);
                return BadRequest("Internal Server Error");
            }
        }
    }
}
