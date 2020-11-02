using DailySharePriceApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailySharePriceApi.Repository
{
    public interface IStockRepository
    {
        public List<Stock> GetStock();
        public Stock GetStockByName(string name);
        public Stock AddStock(Stock s1);
    }
}