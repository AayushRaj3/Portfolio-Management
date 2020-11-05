using CalculateNetWorthApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Services;
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

        static double net = 0;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(NetworthRepository));

        public static List<PortFolioDetails> _portFolioDetails = new List<PortFolioDetails>()
            {
                new PortFolioDetails{
                    PortFolioId=12345,
                    MutualFundList = new List<MutualFundDetails>()
                    {
                        new MutualFundDetails{MutualFundName = "Cred", MutualFundUnits=44},
                        new MutualFundDetails{MutualFundName = "Viva", MutualFundUnits=66}
                    },
                    StockList = new List<StockDetails>()
                    {
                        new StockDetails{StockCount = 19, StockName = "BTC"},
                        new StockDetails{StockCount = 667, StockName = "ETH"}
                    }
                },
                new PortFolioDetails
                {
                    PortFolioId = 789,
                    MutualFundList = new List<MutualFundDetails>()
                    {
                        new MutualFundDetails{MutualFundName = "Udaan", MutualFundUnits=34},
                        new MutualFundDetails{MutualFundName = "Viva", MutualFundUnits=566}
                    },
                    StockList = new List<StockDetails>()
                    {
                        new StockDetails{StockCount = 240, StockName = "BTC"},
                        new StockDetails{StockCount = 46, StockName = "LTC"}
                    }
                }
            };

        //private List<PortFolioDetails> _portFolioDetails;
        /// <summary>
        /// This Will calculate the networth of the Client using the number of stoks and mutual funds he has.
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        /// 

        public async Task<NetWorth> calculateNetWorthAsync(PortFolioDetails portFolioDetails)
        {
            //PortFolioDetails simp = _portFolioDetails.FirstOrDefault(exec => exec.PortFolioId == id);
            Stock st = new Stock();
            MutualFund mf = new MutualFund();
            //double networth = 0;
            NetWorth networth = new NetWorth();
            PortFolioDetails pd = portFolioDetails;
            _log4net.Info("Calculating the networth in the repository method");
            using (var httpClient = new HttpClient())
            {
                if (pd.StockList != null)
                {
                    foreach (StockDetails x in pd.StockList)
                    {
                        using (var response = await httpClient.GetAsync("http://localhost:58451/api/Stock/" + x.StockName))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            st = JsonConvert.DeserializeObject<Stock>(apiResponse);
                        }
                        networth.Networth += x.StockCount * st.StockValue;
                    }
                }
                if (pd.MutualFundList != null)
                {
                    foreach (MutualFundDetails x in pd.MutualFundList)
                    {
                        using (var response = await httpClient.GetAsync("https://localhost:44394/api/MutualFund/" + x.MutualFundName))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            mf = JsonConvert.DeserializeObject<MutualFund>(apiResponse);
                        }
                        networth.Networth += x.MutualFundUnits * mf.MValue;
                    }
                }
            }
            networth.Networth = Math.Round(networth.Networth, 2);
            return networth;
        }

        public AssetSaleResponse sellAssets(List<PortFolioDetails> details)
        {
            NetWorth networth = new NetWorth();
            NetWorth networth2 = new NetWorth();

            PortFolioDetails current = details[0];
            PortFolioDetails toSell = details[1];
            _log4net.Info("Selling the assets");
            foreach(PortFolioDetails x in details)
            {
                if (x == null)
                {
                    return null;
                }
            }
            AssetSaleResponse aq = new AssetSaleResponse();
            networth= calculateNetWorth(current).Result;
            aq.SaleStatus = true;
            foreach (StockDetails x in current.StockList)
            {
                foreach (StockDetails y in toSell.StockList)
                {
                    if (x.StockName == y.StockName)
                    {
                        if (x.StockCount < y.StockCount)
                        {
                            _log4net.Info("Not enough stocks to sell");
                            aq.SaleStatus = false;
                            aq.Networth = networth.Networth;
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
            

            foreach (MutualFundDetails x in current.MutualFundList)
            {
                foreach (MutualFundDetails y in toSell.MutualFundList)
                {
                    if (x.MutualFundName == y.MutualFundName)
                    {
                        if (x.MutualFundUnits < y.MutualFundUnits)
                        {
                            _log4net.Info("Not enough mutualFunds to sell");
                            aq.SaleStatus = false;
                            aq.Networth = networth.Networth;
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

            foreach (PortFolioDetails portfolio in _portFolioDetails)
            {
                if (portfolio.PortFolioId == toSell.PortFolioId)
                {
                    foreach (StockDetails currentstock in portfolio.StockList)
                    {
                        foreach (StockDetails sellstock in toSell.StockList)
                        {
                            if (sellstock.StockName == currentstock.StockName)
                            {
                                currentstock.StockCount = currentstock.StockCount - sellstock.StockCount;
                            }
                        }

                    }


                    foreach (MutualFundDetails currentmutualfund in portfolio.MutualFundList)
                    {
                        foreach (MutualFundDetails sellmutualfund in toSell.MutualFundList)
                        {
                            if (sellmutualfund.MutualFundName == currentmutualfund.MutualFundName)
                            {
                                currentmutualfund.MutualFundUnits = currentmutualfund.MutualFundUnits - sellmutualfund.MutualFundUnits;
                            }
                        }
                    }
                }
            }

            networth2 = calculateNetWorth(toSell).Result;
            aq.Networth = networth.Networth - networth2.Networth;
            net = aq.Networth;
            return aq;
        }
        public async Task<NetWorth> calculateNetWorth(PortFolioDetails pd)
        {
            NetWorth _networth = new NetWorth();

            Stock st = new Stock();
            MutualFund mf = new MutualFund();
            double networth = 0;
            
            using (var httpClient = new HttpClient())
            {
                if (pd.StockList != null)
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
                }
                if (pd.MutualFundList != null)
                {
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
            }
            networth = Math.Round(networth, 2);
            _networth.Networth = networth;
            return _networth;
        }


        public PortFolioDetails GetPortFolioDetailsByID(int id)
        {
            return _portFolioDetails.FirstOrDefault(e => e.PortFolioId == id);
        }
    }
}
