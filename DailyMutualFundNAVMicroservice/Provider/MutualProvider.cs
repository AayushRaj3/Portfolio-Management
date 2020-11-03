using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyMutualFundNAVMicroservice.Provider;
using DailyMutualFundNAVMicroservice.Models;
using DailyMutualFundNAVMicroservice.Repository;

namespace DailyMutualFundNAVMicroservice.Provider
{
    public class MutualProvider:IMutualProvider
    {
        private readonly IMutualFund repo;
        public MutualProvider(IMutualFund repo)
        {
            this.repo = repo;
        }
        public MutualFundDetails GetDailyNAV(string name)
        {
            var data = repo.GetDailyNAV(name);
            if (data==null)
            {
                return null;
            }
            return data;
        }
    }
}
