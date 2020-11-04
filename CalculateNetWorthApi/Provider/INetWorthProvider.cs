using CalculateNetWorthApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculateNetWorthApi.Provider
{
    public interface INetWorthProvider
    {
        public Task<double> calculateNetWorthAsync(PortFolioDetails pd);

        public AssetSaleResponse sellAssets(List<PortFolioDetails> both);
    }
}
