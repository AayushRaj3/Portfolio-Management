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

        private IStockProvider provider;

        public StockController(IStockProvider provider)
        {
            this.provider = provider;
        }

        //[HttpGet]
        //public IActionResult GetStock()
        //{
        //    try
        //    {
        //        _log4net.Info("StockController HttpGet GetStock");
        //        var details = repo.GetStock();
        //        return Ok(details);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

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
                    var result = provider.GetStockByName(name.ToUpper());
                    if (result == null)
                    {
                        _log4net.Info("StockController Invalid Stock Name ");
                        return BadRequest("Invalid Stock Name");
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
                _log4net.Info("Stock COntroller Exception Found - " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //public IActionResult AddStock(Stock s1)
        //{
        //    try
        //    {
        //        var result = repo.AddStock(s1);
        //        if(result == null)
        //        {
        //            return BadRequest("Stock couldn't be added");
        //        }
        //        else
        //        {
        //            return Ok(s1);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
