using AccountingApi.Data.Repository.Interface;
using AccountingApi.Models;
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
        private IPathProvider _pathProvider;

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
                        CompanyId = Convert.ToInt32 (companyId)
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
    }
}
