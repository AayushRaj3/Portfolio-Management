using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyMutualFundNAVMicroservice.Models;
using DailyMutualFundNAVMicroservice.Repository;

namespace DailyMutualFundNAVMicroservice.Repository
{
    public class MutualFundRepository:IMutualFundRepository
    {
        private static List<MutualFundDetails> lst = new List<MutualFundDetails>()
        { 
            new MutualFundDetails { MutualFundId = 1, MutualFundName = "Tata Equity PE Fund", MutualFundValue = 135.84},
            new MutualFundDetails { MutualFundId = 2, MutualFundName = "SBI Nifty Index Fund", MutualFundValue = 100.14},
            new MutualFundDetails { MutualFundId = 3, MutualFundName = "Axis Liquid Fund", MutualFundValue = 2244.45}
        };

        //This will return the MutualFund Details by name
          public MutualFundDetails GetMutualFundByNameRepo(string mutualFundName)
          {
              if (mutualFundName != null)
              {
                  MutualFundDetails data = lst.Where(e => e.MutualFundName == mutualFundName).FirstOrDefault();
                  return data;
              }
              return null;
          }
    }
}