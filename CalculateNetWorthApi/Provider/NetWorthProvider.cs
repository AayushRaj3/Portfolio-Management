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
        private readonly INetWorthRepository _netWorthRepository;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(NetWorthProvider));

        public NetWorthProvider(INetWorthRepository netWorthRepository)
        {
            _netWorthRepository = netWorthRepository;
        }
        public Task<NetWorth> calculateNetWorthAsync(PortFolioDetails portFolioDetails)
        {
            _log4net.Info("Provider called from Controller to calculate the networth");
            var networth = _netWorthRepository.calculateNetWorthAsync(portFolioDetails);
            if (networth.Result.Networth == 0)
            {
                return null;
            }
            return networth;
        }

        public AssetSaleResponse sellAssets(List<PortFolioDetails> listOfAssetsCurrentlyHoldingAndAssetsToSell)
        {
            _log4net.Info("Provider called from Controller to sell some assets");
            AssetSaleResponse assetSaleResponse = _netWorthRepository.sellAssets(listOfAssetsCurrentlyHoldingAndAssetsToSell);
            if (assetSaleResponse == null)
            {
                return null;
            }
            return assetSaleResponse;
        }

        public PortFolioDetails GetPortFolioDetailsByID(int id)
        {
            return _netWorthRepository.GetPortFolioDetailsByID(id);
        }
    }
}
