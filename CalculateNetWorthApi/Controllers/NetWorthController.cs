using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculateNetWorthApi.Models;
using CalculateNetWorthApi.Provider;
using CalculateNetWorthApi.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CalculateNetWorthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetWorthController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(NetWorthController));

        private readonly INetWorthProvider _netWorthProvider;

        public NetWorthController(INetWorthProvider netWorthProvider)
        {
            _netWorthProvider = netWorthProvider;
        }

        [HttpGet("{id}")]
        //[Route("GetPortFolio")]
        public PortFolioDetails GetPortFolioDetailsByID(int id)
        {
            return _netWorthProvider.GetPortFolioDetailsByID(id);
        }

        // GET api/<NetWorthController>/5
        [HttpPost]
        //[Route("GetNetWorth")]
        public IActionResult GetNetWorth(PortFolioDetails portFolioDetails)
        {

            NetWorth _netWorth = new NetWorth();
            _log4net.Info("Calculating the networth");

            try
            {
                //if(HttpContext.Request.Body == null)
                //{
                //    return BadRequest("Please provide a valid PortFolio");

                //}
                //else if (portFolioDetails.ToString() == "")
                //{
                //    return BadRequest("Please provide a valid PortFolio");
                //}

                //else if(portFolioDetails.MutualFundList==null && portFolioDetails.StockList == null)
                //{
                //    _log4net.Info("Both The lists are empty");
                //    return BadRequest("The customer doesn't hold any assets");
                //}

                _netWorth = _netWorthProvider.calculateNetWorthAsync(portFolioDetails).Result;
                return Ok(_netWorth);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<NetWorthController>
        [HttpPost]
        [Route("SellAssets")]
        public IActionResult SellAssets(List<PortFolioDetails> listOfAssetsCurrentlyHoldingAndAssetsToSell)
        {
            try
            {
                _log4net.Info("Selling some assets");
                AssetSaleResponse assetSaleResponse = new AssetSaleResponse();
                if (listOfAssetsCurrentlyHoldingAndAssetsToSell == null)
                {
                    return BadRequest("Please Provide a Valid List of portFolios");
                }
                else
                {
                    assetSaleResponse = _netWorthProvider.sellAssets(listOfAssetsCurrentlyHoldingAndAssetsToSell);
                    if (assetSaleResponse == null)
                    {
                        _log4net.Info("Couldn't be sold because of invalid portfolio");
                        return BadRequest("Please provide a valid list of portfolios");
                    }
                    return Ok(assetSaleResponse);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        


    }
}
