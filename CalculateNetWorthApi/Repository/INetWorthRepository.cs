using CalculateNetWorthApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetWorthApi.Repository
{
    public interface INetWorthRepository
    {
        public Task<double> calculateNetWorthAsync(PortFolioDetails pd);

        public AssetSaleResponse sellAssets(PortFolioDetails current, PortFolioDetails toSell);
    }
}
