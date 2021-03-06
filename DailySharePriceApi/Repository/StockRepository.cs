﻿using DailySharePriceApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DailySharePriceApi.Repository
{
    public class StockRepository : IStockRepository
    {
        public static List<Stock> stocks = new List<Stock>()
        {
            new Stock { StockId = 101, StockName = "BTC", StockValue = 99.95},
            new Stock { StockId = 102, StockName = "ETH", StockValue = 40.2},
            new Stock {StockId = 103, StockName = "LTC", StockValue = 23.6}
        };
       
        public Stock GetStockByNameRepository(string name)
        {
            var stock = stocks.FirstOrDefault(s => s.StockName == name);
            if(stock == null)
            {
                return null;
            }
            else
            {
                return stock;
            }
        }
    }
}
