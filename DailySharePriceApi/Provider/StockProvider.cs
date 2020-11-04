using DailySharePriceApi.Models;
using DailySharePriceApi.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailySharePriceApi.Provider
{
    public class StockProvider : IStockProvider
    {
        private readonly IStockRepository repo;

        public StockProvider(IStockRepository repo)
        {
            this.repo = repo;
        }
        public Stock GetStockByName(string name)
        {
            return repo.GetStockByName(name);
        }
    }
}
