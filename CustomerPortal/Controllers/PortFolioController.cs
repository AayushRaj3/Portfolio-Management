using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        double nw;
        public List<PortFolioDetails> _portFolioDetails = new List<PortFolioDetails>()
            {
                new PortFolioDetails{PortFolioId=12345,
                    MutualFundList = new List<MutualFundDetails>()
                    {
                        new MutualFundDetails{MutualFundName = "Cred", MutualFundUnits=34},
                        new MutualFundDetails{MutualFundName = "Viva", MutualFundUnits=566}
                    },
                    StockList = new List<StockDetails>()
                    {
                        new StockDetails{StockCount = 1, StockName = "BTC"},
                        new StockDetails{StockCount = 6, StockName = "ETH"}
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

        
        public PortFolioController()
        {
           
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
            if (CheckValid())
            {
                CompleteDetails cd = new CompleteDetails();
                PortFolioDetails simp = new PortFolioDetails();
                int id = Convert.ToInt32(HttpContext.Session.GetString("Id"));
                simp = _portFolioDetails.FirstOrDefault(obj => obj.PortFolioId == id);
                cd.PFId = simp.PortFolioId;
                cd.FinalMutualFundList = simp.MutualFundList;
                cd.FinalStockList = simp.StockList;
                nw = 0;
                /*
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44375/api/NetWorth/" + id))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                         nw = Convert.ToDouble(apiResponse);
                    }
                }*/

                StringContent content = new StringContent(JsonConvert.SerializeObject(simp), Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    using (var response = await client.PostAsync("https://localhost:44375/api/NetWorth/GetNetWorth/", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        nw = Convert.ToDouble(apiResponse);
                        //ord = o[0];
                    }
                }
                cd.NetWorth = nw;
                
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
            current = _portFolioDetails.FirstOrDefault(e => e.PortFolioId == id);
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
            if (assetSaleResponse.SaleStatus == true)
            {
                foreach (PortFolioDetails x in _portFolioDetails)
                {
                    if (x.PortFolioId == id)
                    {
                        foreach (StockDetails m in x.StockList)
                        {
                            if (m.StockName == sd.StockName)
                            {
                                m.StockCount = m.StockCount - sd.StockCount;
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
            return View("Reciept", assetSaleResponse);
        }

        [HttpPost]
        public async Task<IActionResult> BuyMutualFund(MutualFundDetails md)
        {
            PortFolioDetails current = new PortFolioDetails();
            PortFolioDetails toSell = new PortFolioDetails();
            int id = Convert.ToInt32(HttpContext.Session.GetString("Id"));
            current = _portFolioDetails.FirstOrDefault(e => e.PortFolioId == id);
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
                using (var response = await client.PostAsync("https://localhost:44375/api/NetWorth/SellAssets/", content))
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
                    if (x.PortFolioId == id)
                    {
                        foreach (MutualFundDetails m in x.MutualFundList)
                        {
                            if (m.MutualFundName == md.MutualFundName)
                            {
                                m.MutualFundUnits = m.MutualFundUnits - md.MutualFundUnits;
                                if (m.MutualFundUnits == 0)
                                {
                                    x.MutualFundList.Remove(m);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            return View("Reciept", assetSaleResponse);
        }



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
    }
}
