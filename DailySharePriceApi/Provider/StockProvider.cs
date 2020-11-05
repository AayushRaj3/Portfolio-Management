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
        private readonly IStockRepository _repo;

        public StockProvider(IStockRepository repo)
        {
            _repo = repo;
        }
        public Stock GetStockByNameProvider(string name)
        {
            var data = _repo.GetStockByNameRepository(name);
            if(data == null)
            {
                return null;
            }
            else
            {
                return data;
            }
        }
    }
}
