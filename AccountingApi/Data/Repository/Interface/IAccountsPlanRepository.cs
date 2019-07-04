using AccountingApi.Dtos.Account;
using AccountingApi.Models;
using AccountingApi.Models.ProcudureDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository.Interface
{
 public  interface IAccountsPlanRepository
    {
         Task<List<AccountsPlan>> ImportFromExcel(int? companyId);
        Task<List<AccountsPlan>> GetAccountsPlans(int? companyId);
        //From Procedure
        Task<List<BalanceSheetReturnDto>> BalanceSheet(int? companyId, DateTime? startDate, DateTime? endDate);
    }
}
