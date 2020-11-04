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

        public NetWorthController(INetWorthProvider ip)
        {
            _netWorthProvider = ip;
        }

        // GET api/<NetWorthController>/5
        //[HttpGet("{id}")]
        [HttpPost]
        [Route("Get")]
        public Task<double> Get(PortFolioDetails pd)
        {
            return _netWorthProvider.calculateNetWorthAsync(pd);
        }

        // POST api/<NetWorthController>
        [HttpPost]
        [Route("Sell")]
        public AssetSaleResponse Post(List<PortFolioDetails> both)
        {
            return _netWorthProvider.sellAssets(both);
        }

    }
}
