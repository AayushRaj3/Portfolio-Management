using CalculateNetWorthApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;

namespace CalculateNetWorthApi.Repository
{ 
    public class NetworthRepository : INetWorthRepository
    {
        private List<PortFolioDetails> _portFolioDetails;
        public Task<Double> calculateNetWorthAsync(PortFolioDetails pd)
        {
            //PortFolioDetails simp = _portFolioDetails.FirstOrDefault(exec => exec.PortFolioId == id);
            return calculateNetWorth(pd) ;
        }

        public AssetSaleResponse sellAssets(List<PortFolioDetails> both)
        {
            AssetSaleResponse aq = new AssetSaleResponse();
            double currentNetWorth = calculateNetWorth(both[0]).Result;
            aq.SaleStatus = true;
            foreach (StockDetails x in both[0].StockList)
            {
                foreach (StockDetails y in both[1].StockList)
                {
                    if (x.StockName == y.StockName)
                    {
                        if (x.StockCount < y.StockCount)
                        {
                            aq.SaleStatus = false;
                            aq.Networth = currentNetWorth;
                            return aq;
                        }
                        x.StockCount = x.StockCount - y.StockCount;
                        /*if (x.StockCount == 0)
                        {
                            both[0].StockList.Remove(x);
                        }*/
                    }
                    break;
                }
            }

            foreach (MutualFundDetails x in both[0].MutualFundList)
            {
                foreach (MutualFundDetails y in both[1].MutualFundList)
                {
                    if (x.MutualFundName == y.MutualFundName)
                    {
                        if (x.MutualFundUnits < y.MutualFundUnits)
                        {
                            aq.SaleStatus = false;
                            aq.Networth = currentNetWorth;
                            return aq;
                        }
                        x.MutualFundUnits = x.MutualFundUnits - y.MutualFundUnits;
                       /* if (x.MutualFundUnits == 0)
                        {
                            both[0].MutualFundList.Remove(x);
                        }*/
                    }
                    break;
                }
            }
            double toSellAmount = calculateNetWorth(both[1]).Result;
            aq.Networth = currentNetWorth - toSellAmount;
            return aq;
        }
        public async Task<double> calculateNetWorth(PortFolioDetails pd)
        {
            
            Stock st = new Stock();
            MutualFund mf = new MutualFund();
            double networth = 0;
            
            using (var httpClient = new HttpClient())
            {
                foreach (StockDetails x in pd.StockList)
                {
                    using (var response = await httpClient.GetAsync("http://localhost:58451/api/Stock/" + x.StockName))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        st = JsonConvert.DeserializeObject<Stock>(apiResponse);
                    }
                    networth += x.StockCount * st.StockValue;
                }

                foreach (MutualFundDetails x in pd.MutualFundList)
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44394/api/MutualFund/" + x.MutualFundName))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        mf = JsonConvert.DeserializeObject<MutualFund>(apiResponse);
                    }
                    networth += x.MutualFundUnits * mf.MValue;
                }
            }
            networth = Math.Round(networth, 2);
            return networth;
        }
    }
}
