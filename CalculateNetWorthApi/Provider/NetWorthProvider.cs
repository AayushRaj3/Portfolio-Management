using CalculateNetWorthApi.Models;
using CalculateNetWorthApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetWorthApi.Provider
{
    public class NetWorthProvider : INetWorthProvider
    {
        private readonly INetWorthRepository repo;

        public NetWorthProvider(INetWorthRepository repo)
        {
            this.repo = repo;
        }
        public Task<double> calculateNetWorthAsync(PortFolioDetails pd)
        {
            return repo.calculateNetWorthAsync(pd);
        }

        public AssetSaleResponse sellAssets(List<PortFolioDetails> both)
        {
            return repo.sellAssets(both);
        }
    }
}
