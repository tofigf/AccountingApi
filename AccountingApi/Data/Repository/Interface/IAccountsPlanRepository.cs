using AccountingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository.Interface
{
 public  interface IAccountsPlanRepository
    {
         Task<List<AccountsPlan>> ImportFromExcel(int? companyId);
    }
}
