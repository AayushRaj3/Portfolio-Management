using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CustomerPortal.Models;
using CustomerPortal.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomerPortal.Controllers
{
    public class PortFolioController : Controller
    {
        static double nw = 0;

        PortFolioDetails _portFolioDetails;
        
        public PortFolioController()
        {
            _portFolioDetails = new PortFolioDetails();
        }
        public bool CheckValid()
        {
            if (HttpContext.Session.GetString("JWTtoken") != null)
            {
                return true;
            }
            return false;
        }

        // GET: PortFolioController
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            NetWorth _netWorth = new NetWorth();
            if (CheckValid())
            {
                CompleteDetails cd = new CompleteDetails();
                PortFolioDetails portFolioDetails = new PortFolioDetails();
                int id = Convert.ToInt32(HttpContext.Session.GetString("Id"));

                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync("https://localhost:44375/api/NetWorth/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        portFolioDetails = JsonConvert.DeserializeObject<PortFolioDetails>(apiResponse);
                        //ord = o[0];
                    }
                }
                StringContent content = new StringContent(JsonConvert.SerializeObject(portFolioDetails), Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    using (var response = await client.PostAsync("https://localhost:44375/api/NetWorth/", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        _netWorth = JsonConvert.DeserializeObject<NetWorth>(apiResponse); ;
                        
                    }
                }
                
                cd.PFId = portFolioDetails.PortFolioId;
                cd.FinalMutualFundList = portFolioDetails.MutualFundList;
                cd.FinalStockList = portFolioDetails.StockList;

                cd.NetWorth = _netWorth.networth;
                
                return View(cd);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Buy(int id)
        {
            SellingViewModel sell = new SellingViewModel();
            sell.PortFolioID = id;
            return View(sell);
        }

        public IActionResult BuyStock(string name)
        {
            //SellingViewModel sell = new SellingViewModel();
            StockDetails sd = new StockDetails();
            sd.StockName = name;
            return View(sd);

        }

        public IActionResult BuyMutualFund(string name)
        {
            MutualFundDetails md = new MutualFundDetails();
            md.MutualFundName = name;
            return View(md);
        }


        [HttpPost]
        public async Task<IActionResult> BuyStock(StockDetails sd)
        {
            PortFolioDetails current = new PortFolioDetails();
            PortFolioDetails toSell = new PortFolioDetails();
            int id = Convert.ToInt32(HttpContext.Session.GetString("Id"));

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:44375/api/NetWorth/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    current = JsonConvert.DeserializeObject<PortFolioDetails>(apiResponse);
                    //ord = o[0];
                }
            }
            toSell.PortFolioId = id;
            toSell.StockList = new List<StockDetails>
            {
                sd
            };
            toSell.MutualFundList = new List<MutualFundDetails>() { };

            List<PortFolioDetails> list = new List<PortFolioDetails>
            {
                current,
                toSell
            };

            AssetSaleResponse assetSaleResponse = new AssetSaleResponse();
            StringContent content = new StringContent(JsonConvert.SerializeObject(list), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                using (var response = await client.PostAsync("https://localhost:44375/api/NetWorth/SellAssets/", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    assetSaleResponse = JsonConvert.DeserializeObject<AssetSaleResponse>(apiResponse);
                    //ord = o[0];
                }
            }
            nw = assetSaleResponse.Networth;
            return View("Reciept", assetSaleResponse);
        }

        [HttpPost]
        public async Task<IActionResult> BuyMutualFund(MutualFundDetails md)
        {
            PortFolioDetails current = new PortFolioDetails();
            PortFolioDetails toSell = new PortFolioDetails();
            int id = Convert.ToInt32(HttpContext.Session.GetString("Id"));

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:44375/api/NetWorth/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    current = JsonConvert.DeserializeObject<PortFolioDetails>(apiResponse);
                    //ord = o[0];
                }
            }
            toSell.PortFolioId = id;
            toSell.MutualFundList = new List<MutualFundDetails>
            {
                md
            };
            toSell.StockList = new List<StockDetails>();

            List<PortFolioDetails> list = new List<PortFolioDetails>
            {
                current,
                toSell
            };

            AssetSaleResponse assetSaleResponse = new AssetSaleResponse();
            StringContent content = new StringContent(JsonConvert.SerializeObject(list), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                using (var response = await client.PostAsync("https://localhost:44375/api/NetWorth/SellAssets", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    assetSaleResponse = JsonConvert.DeserializeObject<AssetSaleResponse>(apiResponse);
                    //ord = o[0];
                }
            }
             
            
            return View("Reciept", assetSaleResponse);
        }

        /*

        [HttpPost]
        public async Task<IActionResult> Buy(SellingViewModel svm)
        {
            PortFolioDetails current = new PortFolioDetails();
            current = _portFolioDetails.FirstOrDefault(e => e.PortFolioId == svm.PortFolioID);
            PortFolioDetails toSell = new PortFolioDetails();
            toSell.PortFolioId = svm.PortFolioID;
            toSell.MutualFundList = new List<MutualFundDetails>
            {
                svm.md
            };
            toSell.StockList = new List<StockDetails>
            {
                svm.sd
            };
            List<PortFolioDetails> list = new List<PortFolioDetails>
            {
                current,
                toSell
            };

            AssetSaleResponse assetSaleResponse = new AssetSaleResponse();
            StringContent content = new StringContent(JsonConvert.SerializeObject(list), Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                using (var response = await client.PostAsync("https://localhost:44375/api/NetWorth/Sell/", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    assetSaleResponse = JsonConvert.DeserializeObject<AssetSaleResponse>(apiResponse);
                    //ord = o[0];
                }
            }
            if (assetSaleResponse.SaleStatus == true)
            {
                foreach (PortFolioDetails x in _portFolioDetails)
                {
                    if (x.PortFolioId == svm.PortFolioID)
                    {
                        foreach (MutualFundDetails m in x.MutualFundList)
                        {
                            if (m.MutualFundName == svm.md.MutualFundName)
                            {
                                m.MutualFundUnits = m.MutualFundUnits - svm.md.MutualFundUnits;
                                if (m.MutualFundUnits == 0)
                                {
                                    x.MutualFundList.Remove(m);
                                }
                                break;
                            }
                        }
                    }
                }

                foreach (PortFolioDetails x in _portFolioDetails)
                {
                    if (x.PortFolioId == svm.PortFolioID)
                    {
                        foreach (StockDetails m in x.StockList)
                        {
                            if (m.StockName == svm.sd.StockName)
                            {
                                m.StockCount = m.StockCount - svm.sd.StockCount;
                                if (m.StockCount == 0)
                                {
                                    x.StockList.Remove(m);
                                }
                                break;
                            }
                        }
                    }
                }

            }
            if (assetSaleResponse.SaleStatus == false)
            {
                ModelState.AddModelError(string.Empty, "You don't have enough assets to sell");
                ViewBag.invalid = "You don't have enough assets to sell";
            }
            return View("Reciept", assetSaleResponse);
        }
        */
    }
}
