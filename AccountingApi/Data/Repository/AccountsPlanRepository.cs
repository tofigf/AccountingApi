using AccountingApi.Data.Repository.Interface;
using AccountingApi.Dtos.Account;
using AccountingApi.Dtos.AccountsPlan;
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

        //OperationCategory
        public async Task<List<OperationCategory>> GetOperationCategories()
        {
           var operationCategory =   await _context.OperationCategories.ToListAsync();

            return operationCategory;
        }
        //ManualJournal
        //Post
        public async Task<ManualJournal> CreateManualJournal(int? companyId, ManualJournal manualJournal)
        {
            if (companyId == null)
                return null;
            if (manualJournal == null)
                return null;

            manualJournal.CreatedAt = DateTime.UtcNow.AddHours(4);
            manualJournal.CompanyId = Convert.ToInt32(companyId);
            await _context.ManualJournals.AddAsync(manualJournal);
            await _context.SaveChangesAsync();

            //AccountPlan
            #region AccountPlan
            AccountsPlan accountDebit = await _context.AccountsPlans.FirstOrDefaultAsync(f => f.Id == manualJournal.AccountDebitId);
            if (accountDebit.Debit == null || accountDebit.Debit == 0)
            {
                accountDebit.Debit = manualJournal.Price;
            }
            else
            {
                accountDebit.Debit += manualJournal.Price;
            }
            AccountsPlan accountkredit = await _context.AccountsPlans.FirstOrDefaultAsync(f => f.Id == manualJournal.AccountKreditId);
            if (accountkredit.Kredit == null || accountkredit.Kredit == 0)
            {
                accountkredit.Kredit = manualJournal.Price;
            }
            else
            {
                accountkredit.Kredit += manualJournal.Price;
            }

            BalanceSheet balanceSheetDebit = new BalanceSheet
            {
                CreatedAt = DateTime.UtcNow.AddHours(4),
            CompanyId = Convert.ToInt32(companyId),
                DebitMoney = manualJournal.Price,
                AccountsPlanId = manualJournal.AccountDebitId,
                ManualJournalId = manualJournal.Id
            };
            await _context.BalanceSheets.AddAsync(balanceSheetDebit);
            BalanceSheet balanceSheetKredit = new BalanceSheet
            {
                CreatedAt = DateTime.UtcNow.AddHours(4),
                CompanyId = Convert.ToInt32(companyId),
                KreditMoney = manualJournal.Price,
                AccountsPlanId = manualJournal.AccountKreditId,
                ManualJournalId = manualJournal.Id
            };
            await _context.BalanceSheets.AddAsync(balanceSheetKredit);
            await _context.SaveChangesAsync();
            #endregion
            return manualJournal;
        }  
        //Get
        public async Task<List<ManualJournal>> GetManualJournals(int? companyId)
        {
            if (companyId == null)
                return null;
            List<ManualJournal> manualJournals =  await  _context.ManualJournals.
                Include(a=>a.AccountsPlanDebit).
                Include(i=>i.AccountsPlanKredit).
                Where(w => w.CompanyId == companyId).ToListAsync();

            if (manualJournals == null)
                return null;

            return manualJournals;
        }
        //Edit Get
        public async Task<ManualJournal>GetEditManualJournal(int? companyId, int? journalId)
        {
            var manual =  await _context.ManualJournals.Include(i=>i.AccountsPlanDebit).
                Include(a=>a.AccountsPlanKredit).
                FirstOrDefaultAsync(f => f.CompanyId == companyId && f.Id == journalId);

            return manual;
        }
        //Edit
        public async Task<ManualJournal>EditManualJournal(ManualJournal manualJournal)
        {
            if (manualJournal == null)
                return null;

            _context.Entry(manualJournal).State = EntityState.Modified;
            _context.Entry(manualJournal).Property(a => a.CompanyId).IsModified = false;
            _context.Entry(manualJournal).Property(a => a.CreatedAt).IsModified = false;
            await _context.SaveChangesAsync();

            return manualJournal;
        }
        // Accounting Update
        public ManualJournalPostDto UpdateManualJournalAccountDebit(int? journalId, int? companyId, ManualJournalPostDto journalPostDto, int? OldDebitId)
        {
            if (journalId == null)
                return null;

            if (journalPostDto == null)
                return null;

            double? dbInvoiceTotalPrice = _context.ManualJournals.FirstOrDefault(f => f.Id == journalId).Price;

            //Debit
            if (OldDebitId == journalPostDto.AccountDebitId)
            {
                //AccountPlan
                AccountsPlan accountDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == OldDebitId);
                if (accountDebit.Debit == null || accountDebit.Debit == 0)
                {
                    accountDebit.Debit = journalPostDto.Price;
                }
                else
                {
                    accountDebit.Debit -= dbInvoiceTotalPrice;
                    _context.SaveChanges();

                    accountDebit.Debit += journalPostDto.Price;
                }
                _context.SaveChanges();
                //Balancsheet
                BalanceSheet balanceSheetDebit = _context.BalanceSheets.
                    FirstOrDefault(f => f.AccountsPlanId == OldDebitId && f.ManualJournalId == journalId);
                if (balanceSheetDebit != null)
                {
                    balanceSheetDebit.DebitMoney = journalPostDto.Price;
                    balanceSheetDebit.AccountsPlanId = journalPostDto.AccountDebitId;
                    _context.SaveChanges();
                }
            }
            else
            {
                //AccountPlan
                AccountsPlan oldAccountDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == OldDebitId);
                oldAccountDebit.Debit -= dbInvoiceTotalPrice;
                _context.SaveChanges();
                AccountsPlan accountDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == journalPostDto.AccountDebitId);
                if (accountDebit.Debit == null || accountDebit.Debit == 0)
                {
                    accountDebit.Debit = journalPostDto.Price;
                }
                else
                {
                    accountDebit.Debit += journalPostDto.Price;
                }
                _context.SaveChanges();
                //Balancsheet
                //remove old balancesheet
                BalanceSheet oldBalanceSheetDebit = _context.BalanceSheets
                    .FirstOrDefault(f => f.ManualJournalId == journalId && f.AccountsPlanId == OldDebitId);
                if (oldBalanceSheetDebit != null)
                {
                    _context.BalanceSheets.Remove(oldBalanceSheetDebit);
                    _context.SaveChanges();
                }

                //new balancesheet

                BalanceSheet balanceSheetDebit = new BalanceSheet
                {
                    CreatedAt = DateTime.Now,
                    CompanyId = Convert.ToInt32(companyId),
                    DebitMoney = journalPostDto.Price,
                    AccountsPlanId = journalPostDto.AccountDebitId,
                    ManualJournalId = journalId
                };
                _context.BalanceSheets.Add(balanceSheetDebit);
                _context.SaveChanges();

            }

            return journalPostDto;
        }
        public ManualJournalPostDto UpdateManualJournalAccountKredit(int? journalId, int? companyId, ManualJournalPostDto journalPostDto, int? OldKeditId)
        {
            if (journalId == null)
                return null;

            if (journalPostDto == null)
                return null;
            double? dbInvoiceTotalPrice = _context.ManualJournals.FirstOrDefault(f => f.Id == journalId).Price;
            //Kredit
            if (OldKeditId == journalPostDto.AccountKreditId)
            {
                //AccountPlann
                AccountsPlan accountkredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == OldKeditId);
                if (accountkredit.Kredit == null || accountkredit.Kredit == 0)
                {
                    accountkredit.Kredit = journalPostDto.Price;
                }
                else
                {
                    accountkredit.Kredit -= dbInvoiceTotalPrice;
                    _context.SaveChanges();

                    accountkredit.Kredit += journalPostDto.Price;
                }
                _context.SaveChanges();
                //Balancsheet
                BalanceSheet balanceSheetKredit = _context.BalanceSheets.
                    FirstOrDefault(f => f.AccountsPlanId == OldKeditId && f.ManualJournalId == journalId);
                if (balanceSheetKredit != null)
                {
                    balanceSheetKredit.KreditMoney = journalPostDto.Price;
                    balanceSheetKredit.AccountsPlanId = journalPostDto.AccountKreditId;
                    _context.SaveChanges();
                }
            }
            else
            {
                //AccountPlan
                AccountsPlan oldAccountKredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == OldKeditId);
                oldAccountKredit.Kredit -= dbInvoiceTotalPrice;
                _context.SaveChanges();
                AccountsPlan accountkredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == journalPostDto.AccountKreditId);
                if (accountkredit.Kredit == null || accountkredit.Kredit == 0)
                {
                    accountkredit.Kredit = journalPostDto.Price;
                }
                else
                {
                    accountkredit.Kredit += journalPostDto.Price;
                }
                _context.SaveChanges();
                //Balancsheet
                //remove old balancesheet
                BalanceSheet oldBalanceSheetKredit = _context.BalanceSheets
                    .FirstOrDefault(f => f.ManualJournalId == journalId && f.AccountsPlanId == OldKeditId);
                if (oldBalanceSheetKredit != null)
                {
                    _context.BalanceSheets.Remove(oldBalanceSheetKredit);
                    _context.SaveChanges();
                }

                //new balancesheet
                BalanceSheet balanceSheetKredit = new BalanceSheet
                {
                    CreatedAt = DateTime.Now,
                    CompanyId = Convert.ToInt32(companyId),
                    KreditMoney = journalPostDto.Price,
                    AccountsPlanId = journalPostDto.AccountKreditId,
                    ManualJournalId = journalId
                };
                _context.BalanceSheets.Add(balanceSheetKredit);
                _context.SaveChanges();
            }

            return journalPostDto;
        }
        //Delete:
        public async Task<ManualJournal> DeleteManualJournal(int? companyId, int? journalId)
        {
            if (companyId == null)
                return null;
            if (journalId == null)
                return null;

            var manualJournal = await _context.ManualJournals.FirstOrDefaultAsync(f => f.Id == journalId && f.CompanyId == companyId);
            if (manualJournal == null)
                return null;

            //Accounting
            #region Accounting
            var balancesheet = await _context.BalanceSheets.Where(w => w.CompanyId == companyId && w.ManualJournalId == journalId).ToListAsync();

            var accountPlanDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == manualJournal.AccountDebitId);
            accountPlanDebit.Debit -= manualJournal.Price;
            _context.SaveChanges();
            var accoutPlanKredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == manualJournal.AccountKreditId);
            accoutPlanKredit.Kredit -= manualJournal.Price;
            _context.SaveChanges();
            #endregion

            if (balancesheet != null)
            {
                _context.BalanceSheets.RemoveRange(balancesheet);
            }
            _context.ManualJournals.Remove(manualJournal);

            await _context.SaveChangesAsync();

            return manualJournal;

        }

        public async Task<List<JournalDto>> GetJournal(int? companyId, DateTime? startDate, DateTime? endDate)
        {
            var JournalSheetQuery = await _context.JournalFromQuery
              .FromSql("exec GetJournal {0},{1},{2}",
              companyId, startDate, endDate).ToListAsync();

            return JournalSheetQuery;
        }
    }
}
