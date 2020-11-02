using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailySharePriceApi.Models;
using DailySharePriceApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailySharePriceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        public IStockRepository repo;
        public StockController(IStockRepository repo)
        {
            this.repo = repo;
        }
        //[HttpGet]
        //public IActionResult GetStock()
        //{
        //    try
        //    {
        //        var details =  repo.GetStock();
        //        return Ok(details);
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet("{name}")]
        public IActionResult GetStockByName(string name)
        {
            try
            {
                if (name == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = repo.GetStockByName(name.ToUpper());
                    if (result == null)
                    {
                        return BadRequest("Invalid Stock Name");
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
            }
            catch(Exception ex)
            {
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
