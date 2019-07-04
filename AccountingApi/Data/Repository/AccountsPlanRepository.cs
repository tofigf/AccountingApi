using AccountingApi.Data.Repository.Interface;
using AccountingApi.Dtos.Account;
using AccountingApi.Models;
using AccountingApi.Models.ProcudureDto;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository
{
    public class AccountsPlanRepository : IAccountsPlanRepository
    {
        private readonly DataContext _context;
        private readonly IPathProvider _pathProvider;

        public AccountsPlanRepository(DataContext context, IPathProvider pathProvider)
        {
            _context = context;
            _pathProvider = pathProvider;
        }

        public async Task<List<AccountsPlan>> ImportFromExcel(int? companyId)
        {
            var path = _pathProvider.MapPath("Files/Template.xlsx");
            if (path == null)
                return null;

            var workbook = new XLWorkbook(path);
            IXLWorksheet ws = workbook.Worksheet(1);
            foreach (var row in ws.RowsUsed())
            {
                int.TryParse(row.Cell(1).Value.ToString(), out int index);

                if (index > 0)
                {
                    AccountsPlan accountsplan = new AccountsPlan
                    {
                        AccPlanNumber = row.Cell(1).Value.ToString(),
                        Name = row.Cell(2).Value.ToString(),
                        Level = Convert.ToInt32(row.Cell(4).Value),
                        Obeysto = row.Cell(5).Value.ToString(),
                        CompanyId = Convert.ToInt32 (companyId),
                        Category = row.Cell(7).Value.ToString(),
                    };
                    _context.AccountsPlans.Add(accountsplan);
                   await _context.SaveChangesAsync();
                }
               
            }
            List<AccountsPlan> accounts = await _context.AccountsPlans.Where(w => w.CompanyId == companyId).ToListAsync();
            if (accounts == null)
                return null;
            return accounts;
        }

        public async Task<List<AccountsPlan>> GetAccountsPlans(int? companyId)
        {
            if (companyId == null)
                return null;
            List<AccountsPlan> accountsPlans =  await  _context.AccountsPlans.Where(w => w.CompanyId == companyId).ToListAsync();
            if (accountsPlans == null)
                return null;

            return accountsPlans;
        }
        //BalanceSheet
        public async Task<List<BalanceSheetReturnDto>> BalanceSheet(int? companyId, DateTime? startDate, DateTime? endDate)
        {
            var balanceSheetQuery = await _context.BalanceSheetDtos
             .FromSql("exec Balance {0},{1},{2}",
             companyId,startDate,endDate).ToListAsync();
            List<BalanceSheetReturnDto> sheetReturnDto = new List<BalanceSheetReturnDto>();

            var balansReturn = balanceSheetQuery.Where(w=>w.allCircleDebit != 0 || w.allCircleKredit != 0
            || w.startCircleDebit != 0 || w.startCircleKredit != 0 || w.endCircleDebit != 0 || w.endCircleKredit != 0
            ).Select(s => new BalanceSheetReturnDto

            {
                AccPlanNumber = s.AccPlanNumber,
                Name = s.Name,
                startCircleDebit = s.startCircleDebit,
                startCircleKredit = s.startCircleKredit,
                allCircleDebit = s.allCircleDebit,
                allCircleKredit = s.allCircleKredit,
                endCircleDebit = s.endCircleDebit,
                endCircleKredit = s.endCircleKredit
            }).ToList();


            return balansReturn;
        }
    }
}
