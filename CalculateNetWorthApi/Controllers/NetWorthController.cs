using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculateNetWorthApi.Models;
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

        private readonly INetWorthRepository _netWorthRepository;

        public NetWorthController(INetWorthRepository netWorthRepository)
        {
            _netWorthRepository = netWorthRepository;
        }

        // GET: api/<NetWorthController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<NetWorthController>/5
        [HttpGet("{pd}")]
        public Task<double> Get(PortFolioDetails pd)
        {
            return _netWorthRepository.calculateNetWorthAsync(pd);
        }

        // POST api/<NetWorthController>
        [HttpPost]
        public AssetSaleResponse Post(PortFolioDetails current, PortFolioDetails toSell)
        {
            return _netWorthRepository.sellAssets(current, toSell);
        }

        // PUT api/<NetWorthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<NetWorthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
