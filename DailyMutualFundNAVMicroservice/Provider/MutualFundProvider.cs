using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyMutualFundNAVMicroservice.Provider;
using DailyMutualFundNAVMicroservice.Models;
using DailyMutualFundNAVMicroservice.Repository;

namespace DailyMutualFundNAVMicroservice.Provider
{
    public class MutualFundProvider:IMutualFundProvider
    {
        private readonly IMutualFundRepository repo;
        public MutualFundProvider(IMutualFundRepository _repo)
        {
            repo = _repo;
        }
        public MutualFundDetails GetMutualFundByNamePro(string name)
        {
            var data = repo.GetMutualFundByNameRepo(name);
            if (data==null)
            {
                return null;
            }
            return data;
        }
    }
}