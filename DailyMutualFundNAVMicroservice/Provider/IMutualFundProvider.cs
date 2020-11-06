using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyMutualFundNAVMicroservice.Repository;
using DailyMutualFundNAVMicroservice.Models;

namespace DailyMutualFundNAVMicroservice.Provider
{
    public interface IMutualFundProvider
    {
        public MutualFundDetails GetMutualFundByNamePro(string name);
    }
}
