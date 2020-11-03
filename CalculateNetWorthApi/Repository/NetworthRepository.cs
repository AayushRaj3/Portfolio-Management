using CalculateNetWorthApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalculateNetWorthApi.Repository
{
    public class NetworthRepository : INetWorthRepository
    {
        public Task<Double> calculateNetWorthAsync(PortFolioDetails pd)
        {
            return calculateNetWorth(pd) ;
        }

        public AssetSaleResponse sellAssets(PortFolioDetails current, PortFolioDetails toSell)
        {
            double currentNetWorth = Convert.ToDouble(calculateNetWorth(current));
            double toSellAmount = Convert.ToDouble(calculateNetWorth(toSell));
            AssetSaleResponse aq = new AssetSaleResponse();
            aq.SaleStatus = true;
            aq.Networth = currentNetWorth - toSellAmount;
            return aq;
        }

        public async static Task<double> calculateNetWorth(PortFolioDetails pd)
        {
            Stock st = new Stock();
            MutualFund mf = new MutualFund();
            double networth = 0;
            using (var httpClient = new HttpClient())
            {
                foreach (StockDetails x in pd.StockList)
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44363/api/empls/" + x.StockName))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        st = JsonConvert.DeserializeObject<Stock>(apiResponse);
                    }
                    networth += x.StockCount * st.StockValue;
                }

                foreach (MutualFundDetails x in pd.MutualFundList)
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44363/api/empls/" + x.MutualFundName))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        mf = JsonConvert.DeserializeObject<MutualFund>(apiResponse);
                    }
                    networth += x.MutualFundUnits * mf.MValue;
                }
            }
            return networth;
        }


    }
}
