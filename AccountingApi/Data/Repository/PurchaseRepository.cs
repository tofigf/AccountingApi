using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingApi.Dtos.Purchase.Expense;
using AccountingApi.Dtos.Purchase.ExpenseInvoice;
using AccountingApi.Models;
using EOfficeAPI.Helpers.Pagination;
using Microsoft.EntityFrameworkCore;

namespace AccountingApi.Data.Repository.Interface
{
    public class PurchaseRepository:IPurchaseRepository
    {
        private readonly DataContext _context;

        public PurchaseRepository(DataContext context)
        {
            _context = context;
        }

        //ExpenseInvoice
        #region ExpenseInvoice

        //Post
        public async Task<ExpenseInvoice> CreateInvoice(ExpenseInvoice invoice, int? companyId)
        {
            if (companyId == null)
                return null;
            if (invoice == null)
                return null;
            if (invoice.ContragentId == null)
                return null;

            invoice.CreatedAt = DateTime.UtcNow.AddHours(4);
            invoice.ResidueForCalc = invoice.TotalPrice;
            if (DateTime.Now > invoice.EndDate && invoice.TotalPrice == invoice.ResidueForCalc)
            {
                invoice.IsPaid = 4;
            }
         
            invoice.CompanyId = Convert.ToInt32(companyId);

            await _context.ExpenseInvoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            //AccountPlan
            #region AccountPlan

            AccountsPlan accountDebit = await _context.AccountsPlans.FirstOrDefaultAsync(f => f.Id == invoice.AccountDebitId);
            if (accountDebit.Debit == null || accountDebit.Debit == 0)
            {
                accountDebit.Debit = invoice.TotalPrice;
            }
            else
            {
                accountDebit.Debit += invoice.TotalPrice;
            }
            AccountsPlan accountkredit = await _context.AccountsPlans.FirstOrDefaultAsync(f => f.Id == invoice.AccountKreditId);
            if (accountkredit.Kredit == null || accountkredit.Kredit == 0)
            {
                accountkredit.Kredit = invoice.TotalPrice;
            }
            else
            {
                accountkredit.Kredit += invoice.TotalPrice;
            }

            BalanceSheet balanceSheetDebit = new BalanceSheet
            {
                CreatedAt = DateTime.Now,
                CompanyId = Convert.ToInt32(companyId),
                DebitMoney = invoice.TotalPrice,
                AccountsPlanId = invoice.AccountDebitId,
                ExpenseInvoiceId = invoice.Id
            };
            await _context.BalanceSheets.AddAsync(balanceSheetDebit);
            BalanceSheet balanceSheetKredit = new BalanceSheet
            {
                CreatedAt = DateTime.Now,
                CompanyId = Convert.ToInt32(companyId),
                KreditMoney = invoice.TotalPrice,
                AccountsPlanId = invoice.AccountKreditId,
                ExpenseInvoiceId = invoice.Id
            };
            await _context.BalanceSheets.AddAsync(balanceSheetKredit);

            await _context.SaveChangesAsync();
            #endregion

            return invoice;
        }
        public async Task<List<ExpenseInvoiceItem>> CreateInvoiceItems(List<ExpenseInvoiceItem> items, int? invoiceId)
        {
            if (invoiceId == null)
                return null;

            foreach (var item in items)
            {
                item.ExpenseInvoiceId = invoiceId;

                await _context.ExpenseInvoiceItems.AddAsync(item);
                await _context.SaveChangesAsync();
            }

            return items;
        }

        //Check:
        #region Check
        public async Task<bool> CheckExpenseInvoice(int? currentUserId, int? companyId)
        {
            if (currentUserId == null)
                return true;
            if (companyId == null)
                return true;
            if (await _context.ExpenseInvoices.AnyAsync(a => a.CompanyId == companyId && a.Company.UserId != currentUserId))
                return true;

            return false;
        }
        public async Task<bool> CheckExpenseInvoiceProductId(List<ExpenseInvoiceItem> invoiceItems)
        {
            foreach (var p in invoiceItems)
            {
                if (await _context.Products.FirstOrDefaultAsync(a => a.Id == p.ProductId) == null)
                {
                    return true;
                }
            }

            return false;
        }
        public async Task<bool> CheckExpenseInvoiceId(int? productId, int? companyId)
        {
            if (companyId == null)
                return true;
            if (productId == null)
                return true;
            if (await _context.ExpenseInvoices.AnyAsync(a => a.CompanyId != companyId && a.Id == productId))

                return true;

            return false;
        }
        public bool CheckInvoiceNegativeValue(ExpenseInvoice invoice, List<ExpenseInvoiceItem> items)
        {
            if (invoice.TotalPrice < 0 || invoice.TotalTax < 0 || invoice.Sum < 0)
            {
                return true;
            }
            foreach (var item in items)
            {
                if (item.Price < 0 || item.Qty < 0 || item.TotalOneProduct < 0)
                {
                    return true;
                }
            }

            return false;
        }
        public async Task<bool> CheckExpenseInvoiceItem(int? invoiceId, List<ExpenseInvoiceItem> invoiceItems)
        {
            foreach (var item in invoiceItems.Where(w => w.Id != 0))
            {
                ExpenseInvoiceItem dbInvoiceItem = await _context.ExpenseInvoiceItems.AsNoTracking()
                     .FirstOrDefaultAsync(w => w.ExpenseInvoiceId == invoiceId && w.Id == item.Id);

                if (dbInvoiceItem == null)
                {
                    return true;
                }

            }
            return false;
        }
        #endregion

        //Get
        //get all invoice
        public async Task<List<ExpenseInvoiceGetDto>> GetInvoice(PaginationParam productParam, int? companyId)
        {
            if (companyId == null)
                return null;
            List<ExpenseInvoice> invoices = await _context.ExpenseInvoices.Where(w => w.CompanyId == companyId)
            .OrderByDescending(d => d.Id).ToListAsync();
            List<Contragent> contragents = await _context.Contragents
        .OrderByDescending(d => d.Id).ToListAsync();

            var invoicecon = invoices.Join(contragents, m => m.ContragentId, m => m.Id, (inv, con) => new ExpenseInvoiceGetDto
            {
                ContragentCompanyName = con.CompanyName,
                ExpenseInvoiceNumber = inv.ExpenseInvoiceNumber,
                TotalPrice = inv.TotalPrice,
                PreparingDate = inv.PreparingDate,
                EndDate = inv.EndDate,
                IsPaid =inv.IsPaid,
                Id = inv.Id
            }).ToList();

            return invoicecon;
        }
        //used in update action
        public async Task<ExpenseInvoice> GetEditInvoice(int? invoiceId, int? companyId)
        {
            if (invoiceId == null)
                return null;
            if (companyId == null)
                return null;

            ExpenseInvoice invoice = await _context.ExpenseInvoices.FirstOrDefaultAsync(f => f.Id == invoiceId && f.CompanyId == companyId);

            return invoice;
        }
        //used in update action
        public async Task<List<ExpenseInvoiceItem>> GetEditInvoiceItem(int? invoiceId)
        {
            if (invoiceId == null)
                return null;

            List<ExpenseInvoiceItem> invoiceItems = await _context.ExpenseInvoiceItems.Where(w => w.ExpenseInvoiceId == invoiceId).AsNoTracking().ToListAsync();

            return invoiceItems;
        }
        // detail used in get edit invoice 
        public async Task<ExpenseInvoice> GetDetailInvoice(int? invoiceId, int? companyId)
        {
            if (invoiceId == null)
                return null;
            if (companyId == null)
                return null;

            ExpenseInvoice invoice = await _context.ExpenseInvoices.Include(i => i.Company).Include(i => i.Tax).Include(i => i.ExpenseInvoiceItems).ThenInclude(a => a.Product).FirstOrDefaultAsync(f => f.Id == invoiceId && f.CompanyId == companyId);

            return invoice;
        }
        public async Task<Contragent> GetContragentInvoice(int? companyId, int? invoiceId)
        {

            Contragent contragent = await _context.Contragents.SingleOrDefaultAsync(c => c.CompanyId == companyId &&

         c.ExpenseInvoices.SingleOrDefault(w => w.Id == invoiceId) != null);

            return contragent;
        }
        //Put
        public async Task<ExpenseInvoice> EditInvoice(ExpenseInvoice invoice, List<ExpenseInvoiceItem> invoiceItems, int? invoiceId)
        {
            if (invoice == null)
                return null;
            var invoiceforpaidmoney = _context.ExpenseInvoices.Find(invoice.Id);
            if (DateTime.Now > invoice.EndDate && invoice.TotalPrice == invoice.ResidueForCalc)
            {
                invoiceforpaidmoney.IsPaid = 4;
            }
            else if (DateTime.Now <= invoice.EndDate && invoice.TotalPrice == invoice.ResidueForCalc)
            {
                invoiceforpaidmoney.IsPaid = 1;
            }
            else if (DateTime.Now <= invoice.EndDate && invoice.TotalPrice != invoice.ResidueForCalc)
            {
                invoiceforpaidmoney.IsPaid = 1;
            }
            else if (DateTime.Now > invoice.EndDate && invoice.TotalPrice != invoice.ResidueForCalc)
            {
                invoiceforpaidmoney.IsPaid = 1;
            }
            _context.SaveChanges();

            //update invoice
            _context.Entry(invoice).State = EntityState.Modified;
            _context.Entry(invoice).Property(a => a.CompanyId).IsModified = false;
            _context.Entry(invoice).Property(a => a.CreatedAt).IsModified = false;
            _context.Entry(invoice).Property(a => a.IsDeleted).IsModified = false;

            //_context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();

            //Update IncomeItems
            foreach (var item in invoiceItems.Where(w => w.Id != 0))
            {
                _context.Entry(item).State = EntityState.Modified;

                _context.Entry(item).Property(a => a.ExpenseInvoiceId).IsModified = false;
            }
            _context.SaveChanges();
            foreach (var inv in invoiceItems.Where(w => w.Id == 0))
            {
                ExpenseInvoiceItem invoiceItem = new ExpenseInvoiceItem
                {
                    Qty = inv.Qty,
                    Price = inv.Price,
                    TotalOneProduct = inv.TotalOneProduct,
                    ProductId = inv.ProductId,
                    ExpenseInvoiceId = Convert.ToInt32(invoiceId)
                };
                _context.ExpenseInvoiceItems.Add(invoiceItem);
            }
            _context.SaveChanges();

            // find invoiceById for equal totaprice to resdueForCalc.. because of correct calculating
            var foundinvoice = await _context.ExpenseInvoices.FindAsync(invoiceId);
            // for equal
            foundinvoice.ResidueForCalc = foundinvoice.TotalPrice;

            _context.SaveChanges();
            // for deleting incomesitems
            var expenseItems = await _context.ExpenseItems.Where(f => f.ExpenseInvoiceId == invoiceId).ToListAsync();
            // for deleting income
            //there is bug when deleting income ,invoiceId maybe declared difference  
             var expense = _context.Expenses.FirstOrDefault(f => f.Id == f.ExpenseItems.FirstOrDefault(a => a.ExpenseInvoiceId == invoiceId).ExpenseId);

            if (expenseItems.Count > 0)
            {
                foreach (var item in expenseItems)
                {
                    //Accounting
                    #region Accounting

                    var balancesheet = await _context.BalanceSheets.Where(w => w.ExpenseItemId == item.Id).ToListAsync();

                    var accountPlanDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == item.AccountDebitId);
                    accountPlanDebit.Debit -= item.PaidMoney;
                    _context.SaveChanges();
                    var accoutPlanKredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == item.AccountKreditId);
                    accoutPlanKredit.Kredit -= item.PaidMoney;
                    _context.SaveChanges();
                    if (balancesheet != null)
                    {
                        _context.BalanceSheets.RemoveRange(balancesheet);
                    }

                    #endregion
                    //For Deleting Expenses 
                    var dbincome = _context.Expenses.Include(s => s.ExpenseItems).FirstOrDefault(w => w.CompanyId == expense.CompanyId);

                    if (dbincome.ExpenseItems.Count() == 1)
                    {
                        //removing expenseItems
                        _context.ExpenseItems.Remove(item);

                        _context.SaveChanges();

                        //Remove Expenses
                        _context.Expenses.Remove(expense);
                        _context.SaveChanges();

                    }
                    else
                    {
                        //removing expenseItems
                        _context.ExpenseItems.Remove(item);

                        _context.SaveChanges();
                    }
                }
            }

            return invoice;
        }
        //Delete:DeleteInvoiceItem
        public async Task<ExpenseInvoiceItem> DeleteInvoiceItem(int? invoiceItemId)
        {
            if (invoiceItemId == null)
                return null;
            //InvoicesItems
            var expenseInvoiceItem = await _context.ExpenseInvoiceItems.Include(i => i.ExpenseInvoice).FirstOrDefaultAsync(f => f.Id == invoiceItemId);
          
            if (expenseInvoiceItem == null)
                return null;
            //Invoice
            var expenseInvoice = _context.ExpenseInvoices.Include(t => t.Tax).FirstOrDefault(f => f.Id == expenseInvoiceItem.ExpenseInvoice.Id);
            //New Invoice Sum value
            expenseInvoice.Sum -= expenseInvoiceItem.Price * expenseInvoiceItem.Qty;
            //New Invoice TotalTax value
            expenseInvoice.TotalTax = expenseInvoice.Sum * expenseInvoice.Tax.Rate / 100;
            //New Invoice TotalPrice value
            expenseInvoice.TotalPrice = expenseInvoice.Sum + expenseInvoice.TotalTax;
            //New Invoice ResidueForCalc value
            expenseInvoice.ResidueForCalc = expenseInvoice.Sum + expenseInvoice.TotalTax;

            _context.SaveChanges();
  
            //Accounting
            #region Accounting
            var balancesheetDebit = _context.BalanceSheets.FirstOrDefault(w => w.InvoiceId == expenseInvoice.Id && w.AccountsPlanId == expenseInvoice.AccountDebitId);
            balancesheetDebit.DebitMoney = expenseInvoice.TotalPrice;
            _context.SaveChanges();
            var balancesheetKredit = _context.BalanceSheets.FirstOrDefault(w => w.InvoiceId == expenseInvoice.Id && w.AccountsPlanId == expenseInvoice.AccountKreditId);
            balancesheetKredit.KreditMoney = expenseInvoice.TotalPrice;
            _context.SaveChanges();

            var accountPlanDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == expenseInvoice.AccountDebitId);
            accountPlanDebit.Debit = expenseInvoice.TotalPrice;
            _context.SaveChanges();
            var accoutPlanKredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == expenseInvoice.AccountKreditId);
            accoutPlanKredit.Kredit = expenseInvoice.TotalPrice;
            _context.SaveChanges();
            #endregion

            //Remove InvoiceItems
            _context.ExpenseInvoiceItems.Remove(expenseInvoiceItem);

            await _context.SaveChangesAsync();

            return expenseInvoiceItem;
        }
        //Delete:DeleteInvoice
        public async Task<ExpenseInvoice> DeleteInvoice(int? companyId, int? invoiceId)
        {
            if (companyId == null)
                return null;
            if (invoiceId == null)
                return null;

            var expenseInvoice = await _context.ExpenseInvoices.FirstOrDefaultAsync(f => f.Id == invoiceId && f.CompanyId == companyId);

            if (expenseInvoice == null)
                return null;

            var expenseInvoiceItems = await _context.ExpenseInvoiceItems.Where(w => w.ExpenseInvoiceId == invoiceId).ToListAsync();

            var expenseItems = await _context.ExpenseItems.Where(w => w.ExpenseInvoiceId == invoiceId).ToListAsync();

            //Accounting
            #region Accounting
            var balancesheet = await _context.BalanceSheets.Where(w => w.CompanyId == companyId && w.InvoiceId == invoiceId).ToListAsync();

            var accountPlanDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == expenseInvoice.AccountDebitId);
            accountPlanDebit.Debit -= expenseInvoice.TotalPrice;
            _context.SaveChanges();
            var accoutPlanKredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == expenseInvoice.AccountKreditId);
            accoutPlanKredit.Kredit -= expenseInvoice.TotalPrice;
            _context.SaveChanges();
            #endregion

            if (expenseInvoiceItems != null)
            {
                _context.ExpenseInvoiceItems.RemoveRange(expenseInvoiceItems);
            }
            if (expenseItems != null)
            {
                _context.ExpenseItems.RemoveRange(expenseItems);

            }
            if (balancesheet != null)
            {
                _context.BalanceSheets.RemoveRange(balancesheet);
            }

            _context.ExpenseInvoices.Remove(expenseInvoice);

            await _context.SaveChangesAsync();

            return expenseInvoice;

        }
        // Accounting Update
        public ExpenseInvoicePutDto UpdateInvoiceAccountDebit(int? invoiceId, int? companyId, ExpenseInvoicePutDto invoice, int? OldDebitId)
        {
            if (invoiceId == null)
                return null;

            if (invoice == null)
                return null;

            double? dbInvoiceTotalPrice = _context.ExpenseInvoices.FirstOrDefault(f => f.Id == invoiceId).TotalPrice;

            //Debit
            if (OldDebitId == invoice.AccountDebitId)
            {
                //AccountPlan
                AccountsPlan accountDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == OldDebitId);
                if (accountDebit.Debit == null || accountDebit.Debit == 0)
                {
                    accountDebit.Debit = invoice.TotalPrice;
                }
                else
                {
                    accountDebit.Debit -= dbInvoiceTotalPrice;
                    _context.SaveChanges();

                    accountDebit.Debit += invoice.TotalPrice;
                }
                _context.SaveChanges();
                //Balancsheet
                BalanceSheet balanceSheetDebit = _context.BalanceSheets.
                    FirstOrDefault(f => f.AccountsPlanId == OldDebitId && f.ExpenseInvoiceId == invoiceId);
                if(balanceSheetDebit != null)
                {
                    balanceSheetDebit.DebitMoney = invoice.TotalPrice;
                    balanceSheetDebit.AccountsPlanId = invoice.AccountDebitId;
                    _context.SaveChanges();
                }
            }

            else
            {
                //AccountPlan
                AccountsPlan oldAccountDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == OldDebitId);
                oldAccountDebit.Debit -= dbInvoiceTotalPrice;
                _context.SaveChanges();
                AccountsPlan accountDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == invoice.AccountDebitId);
                if (accountDebit.Debit == null || accountDebit.Debit == 0)
                {
                    accountDebit.Debit = invoice.TotalPrice;
                }
                else
                {
                    accountDebit.Debit += invoice.TotalPrice;
                }
                _context.SaveChanges();
                //Balancsheet
                //remove old balancesheet
                BalanceSheet oldBalanceSheetDebit = _context.BalanceSheets
                    .FirstOrDefault(f => f.ExpenseInvoiceId == invoiceId && f.AccountsPlanId == OldDebitId);
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
                    DebitMoney = invoice.TotalPrice,
                    AccountsPlanId = invoice.AccountDebitId,
                    ExpenseInvoiceId = invoiceId
                };
                _context.BalanceSheets.Add(balanceSheetDebit);
                _context.SaveChanges();

            }

            return invoice;
        }
        public ExpenseInvoicePutDto UpdateInvoiceAccountKredit(int? invoiceId, int? companyId, ExpenseInvoicePutDto invoice, int? OldKeditId)
        {
            if (invoiceId == null)
                return null;

            if (invoice == null)
                return null;
            double? dbInvoiceTotalPrice = _context.ExpenseInvoices.FirstOrDefault(f => f.Id == invoiceId).TotalPrice;
            //Kredit
            if (OldKeditId == invoice.AccountKreditId)
            {

                //AccountPlann
                AccountsPlan accountkredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == OldKeditId);
                if (accountkredit.Kredit == null || accountkredit.Kredit == 0)
                {
                    accountkredit.Kredit = invoice.TotalPrice;
                }
                else
                {
                    accountkredit.Kredit -= dbInvoiceTotalPrice;
                    _context.SaveChanges();

                    accountkredit.Kredit += invoice.TotalPrice;
                }
                _context.SaveChanges();
                //Balancsheet
                BalanceSheet balanceSheetKredit = _context.BalanceSheets.
                    FirstOrDefault(f => f.AccountsPlanId == OldKeditId && f.ExpenseInvoiceId == invoiceId);
                if(balanceSheetKredit != null)
                {
                    balanceSheetKredit.KreditMoney = invoice.TotalPrice;
                    balanceSheetKredit.AccountsPlanId = invoice.AccountKreditId;
                    _context.SaveChanges();
                }
            }
            else
            {
                //AccountPlan
                AccountsPlan oldAccountKredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == OldKeditId);
                oldAccountKredit.Kredit -= dbInvoiceTotalPrice;
                _context.SaveChanges();
                AccountsPlan accountkredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == invoice.AccountKreditId);
                if (accountkredit.Kredit == null || accountkredit.Kredit == 0)
                {
                    accountkredit.Kredit = invoice.TotalPrice;
                }
                else
                {
                    accountkredit.Kredit += invoice.TotalPrice;
                }
                _context.SaveChanges();
                //Balancsheet
                //remove old balancesheet
                BalanceSheet oldBalanceSheetKredit = _context.BalanceSheets
                    .FirstOrDefault(f => f.ExpenseInvoiceId == invoiceId && f.AccountsPlanId == OldKeditId);
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
                    KreditMoney = invoice.TotalPrice,
                    AccountsPlanId = invoice.AccountKreditId,
                    ExpenseInvoiceId = invoiceId
                };
                _context.BalanceSheets.Add(balanceSheetKredit);
                _context.SaveChanges();
            }

            return invoice;
        }

        #endregion

        //Expense
        #region Expense
        //Get: 
        public List<ExpenseExInvoiceGetDto> GetExpenseInvoiceByContragentId(int? contragentId, int? companyId)
        {
            if (contragentId == null)
                return null;
            List<ExpenseExInvoiceGetDto> datas = (from inv in _context.ExpenseInvoices
                                                   //join itemincome in _context.IncomeItems
                                                   //on inv.Id equals itemincome.InvoiceId
                                                   //Left join for take residue in table income
                                                   //into sr
                                                   //from x in sr.DefaultIfEmpty()
                                               where inv.ContragentId == contragentId && inv.CompanyId == companyId && inv.IsPaid != 3
                                               select new ExpenseExInvoiceGetDto
                                               {
                                                   ExpenseInvoiceId = inv.Id,
                                                   ExpenseInvoiceNumber = inv.ExpenseInvoiceNumber,
                                                   TotalPrice = inv.TotalPrice,
                                                   Residue = inv.ResidueForCalc,
                                                   PreparingDate = inv.PreparingDate,
                                                   EndDate = inv.EndDate,
                                                   AccountDebitId =inv.AccountDebitId,
                                                   AccountKreditId =inv.AccountKreditId,
                                                   
                                               }).ToList();
            return datas;
        }
        public async Task<List<ExpenseGetDto>> GetExpense(PaginationParam productParam, int? companyId)
        {
            if (companyId == null)
                return null;

            var incomeItems = await _context.ExpenseItems.Include(i => i.Expense).Include(a => a.ExpenseInvoice)
                    .ThenInclude(t => t.Contragent)
                    .Where(w => w.Expense.CompanyId == companyId)
                    .OrderByDescending(d => d.Id)
                    .GroupBy(p => p.ExpenseInvoiceId)
                    .Select(g => new
                    {
                        first = g.First(),
                        sum = g.Sum(s => s.PaidMoney)
                    }

                    ).ToListAsync();

            var joinIncome = incomeItems.Select(s => new ExpenseGetDto
            {
                ContragentCompanyName = s.first.Expense.Contragent.CompanyName,
                ContragentFullname = s.first.Expense.Contragent.Fullname,
                ExpenseInvoiceNumber = s.first.InvoiceNumber,
                Id = s.first.Expense.Id,
                IsBank = s.first.IsBank,
                TotalPrice = s.first.Expense.TotalPrice,
                PaidMoney = s.first.PaidMoney,
                Residue = s.first.ExpenseInvoice.ResidueForCalc,
                CreatedAt = s.first.Expense.CreatedAt,
                TotalOneInvoice = s.first.TotalOneInvoice,
                InvoiceId = s.first.ExpenseInvoiceId,
                SumPaidMoney = s.sum
            }).ToList();

            return joinIncome;
        }
        //https://localhost:44317/api/income/geteditincome
        //get edit income
        public Task<ExpenseInvoice> GetExpenseExpenseItem(int? companyId, int? invoiceId)
        {
            if (invoiceId == null)
                return null;
            if (companyId == null)
                return null;
            var invoice = _context.ExpenseInvoices.Include(i => i.ExpenseItems).ThenInclude(t => t.Expense).Include(c => c.Contragent)
                .FirstOrDefaultAsync(f => f.CompanyId == companyId && f.Id == invoiceId);
            if (invoice == null)
                return null;

            return invoice;
        }
        //get for editing income
        public async Task<Expense> GetEditExpense(int? expenseId, int? companyId)
        {
            if (expenseId == null)
                return null;
            if (companyId == null)
                return null;

            Expense expense = await _context.Expenses.FirstOrDefaultAsync(f => f.CompanyId == companyId && f.Id == expenseId);

            return expense;
        }
        public async Task<List<ExpenseItem>> GetEditExpenseItems(int? invoiceId)
        {
            if (invoiceId == null)
                return null;

            List<ExpenseItem> expenseItems = await _context.ExpenseItems
                .Where(w => w.ExpenseInvoiceId == invoiceId).AsNoTracking().ToListAsync();

            return expenseItems;
        }
        //Post:
        public async Task<Expense> CreateExpense(int? companyId, int? contragentId, Expense expense, List<ExpenseItem> expenseItems)
        {
            if (companyId == null)
                return null;

            if (contragentId == null)
                return null;

            expense.CreatedAt = DateTime.UtcNow.AddHours(4);
            expense.CompanyId = Convert.ToInt32(companyId);
            expense.ContragentId = Convert.ToInt32(contragentId);
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();

            //foreach (var id in Ids)
            //{
            foreach (var inc in expenseItems)
            {
                //invoice for update IsPaid
                var invoice = _context.ExpenseInvoices.Find(inc.ExpenseInvoiceId);

                if (invoice == null)
                    return null;
                if (invoice.ResidueForCalc <= inc.PaidMoney)
                {
                    //1=planlinib, 2 = gozlemede,3=odenilib
                    invoice.IsPaid = 3;
                }

                else if (invoice.ResidueForCalc > inc.PaidMoney)
                {
                    //1=planlinib, 2 = gozlemede,3=odenilib
                    invoice.IsPaid = 2;
                }
                else
                {
                    //1=planlinib, 2 = gozlemede,3=odenilib
                    invoice.IsPaid = 1;
                }
                if (inc.PaidMoney != null)
                {
                    invoice.ResidueForCalc -= inc.PaidMoney;
                }
                _context.SaveChanges();

                inc.ExpenseId = expense.Id;

                await _context.ExpenseItems.AddAsync(inc);

                // }

                //AccountPlan
                #region Account
                AccountsPlan accountDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == inc.AccountDebitId);
                if (accountDebit.Debit == null || accountDebit.Debit == 0)
                {
                    accountDebit.Debit = inc.PaidMoney;
                }
                else
                {
                    accountDebit.Debit += inc.PaidMoney;
                }

                _context.SaveChanges();

                AccountsPlan accountkredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == inc.AccountKreditId);
                if (accountkredit.Kredit == null || accountkredit.Kredit == 0)
                {
                    accountkredit.Kredit = inc.PaidMoney;
                }
                else
                {
                    accountkredit.Kredit += inc.PaidMoney;
                }

                _context.SaveChanges();
                BalanceSheet balanceSheetDebit = new BalanceSheet
                {
                    CreatedAt = DateTime.Now,
                    CompanyId = Convert.ToInt32(companyId),
                    DebitMoney = inc.PaidMoney,
                    AccountsPlanId = inc.AccountDebitId,
                    ExpenseItemId = inc.Id
                };
                _context.BalanceSheets.Add(balanceSheetDebit);
                _context.SaveChanges();
                BalanceSheet balanceSheetKredit = new BalanceSheet
                {
                    CreatedAt = DateTime.Now,
                    CompanyId = Convert.ToInt32(companyId),
                    KreditMoney = inc.PaidMoney,
                    AccountsPlanId = inc.AccountKreditId,
                    ExpenseItemId = inc.Id
                };
                _context.BalanceSheets.Add(balanceSheetKredit);
                _context.SaveChanges();
                #endregion
            }
            await _context.SaveChangesAsync();

            return expense;
        }
        //Put
        //so far stopped this method
        public async Task<ExpenseItem> EditExpense(List<ExpenseItemGetDto> itemGetDtos,int? invoiceId)
        {
            if (invoiceId == null)
                return null;
            //Update ExpenseItems
            foreach (var item in itemGetDtos)
            {
                double? sumPaidMoney = itemGetDtos.Where(w => w.ExpenseInvoiceId == invoiceId).Sum(s => s.PaidMoney);
                var expenseitem = _context.ExpenseItems.Find(item.Id);
                var invoice = _context.ExpenseInvoices.Find(invoiceId);
                invoice.ResidueForCalc += expenseitem.PaidMoney;
                _context.SaveChanges();

                expenseitem.PaidMoney = item.PaidMoney;
                expenseitem.IsBank = item.IsBank;
                expenseitem.AccountDebitId = item.AccountDebitId;
                expenseitem.AccountKreditId = item.AccountKreditId;
                _context.SaveChanges();

                invoice.ResidueForCalc -= item.PaidMoney;
                _context.SaveChanges();

                //invoice for update IsPaid
                if (invoice == null)
                    return null;

                if (invoice.TotalPrice <= sumPaidMoney)
                {
                    //1=planlinib, 2 = gozlemede, 3=odenilib
                    invoice.IsPaid = 3;
                }
                else if (invoice.TotalPrice > sumPaidMoney)
                {
                    //1=planlinib, 2 = gozlemede, 3=odenilib
                    invoice.IsPaid = 2;
                }
                else
                {
                    //1=planlinib, 2 = gozlemede, 3=odenilib
                    invoice.IsPaid = 1;
                }

                //if (invoice.ResidueForCalc != null)
                //{
                //    invoice.ResidueForCalc = invoice.TotalPrice - sumPaidMoney;
                //}
            }
            await _context.SaveChangesAsync();

            return null;
        }
        //Check:
        #region Check
        public async Task<bool> CheckExpense(int? currentUserId, int? companyId)
        {
            if (currentUserId == null)
                return true;
            if (companyId == null)
                return true;
            if (await _context.Expenses.AnyAsync(a => a.CompanyId == companyId && a.Company.UserId != currentUserId))
                return true;

            return false;
        }
        //check invoice total price with paidmoney
        public async Task<bool> CheckExpenseEqualingInvoiceTotalPriceForUpdate(List<ExpenseItemGetDto> expenseItems)
        {
            //total paidmoney
            double? TotalPaidMoney = 0;
            foreach (var item in expenseItems)
            {
                var dbincomeitems = _context.ExpenseItems.Where(w => w.Id == item.Id).ToList();
                foreach (var dbitem in dbincomeitems)
                {
                    var expenseItemsForPaidMoney = await _context.ExpenseInvoices.FirstOrDefaultAsync(f => f.Id == dbitem.ExpenseInvoiceId);
                    if (expenseItemsForPaidMoney == null)
                        return true;
                    TotalPaidMoney += item.PaidMoney;
                    //checkig totalpaidmoney and totaloneinvoice
                    if (expenseItemsForPaidMoney.TotalPrice < TotalPaidMoney)
                    {
                        return true;
                    }
                    if (_context.ExpenseItems.FirstOrDefault(f => f.Id == item.Id) == null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public async Task<bool> CheckExpenseEqualingInvoiceTotalPriceForCreate(List<ExpenseItem> incomeItems)
        {
            foreach (var item in incomeItems)
            {
                var expenseItemsForPaidMoney = await _context.ExpenseInvoices.Where(f => f.Id == item.ExpenseInvoiceId).ToListAsync();
                if (expenseItemsForPaidMoney == null)
                    return true;
                //total paidmoney
                double? TotalPaidMoney = 0;
                foreach (var exppaid in expenseItemsForPaidMoney)
                {
                    TotalPaidMoney += item.PaidMoney;

                    //checkig totalpaidmoney and totaloneinvoice
                    if (exppaid.TotalPrice < TotalPaidMoney)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public async Task<bool> CheckIncomeContragentIdInvoiceId(int? contragentId, int? companyId)
        {
            if (contragentId == null)
                return true;

            if (await _context.Contragents.FirstOrDefaultAsync(a => a.CompanyId == companyId && a.Id == contragentId) == null)
            {
                return true;
            }

            return false;
        }
        public bool CheckExpenseNegativeValue(Expense expense, List<ExpenseItem> expenses)
        {
            if (expense.TotalPrice < 0)
            {
                return true;
            }
            foreach (var item in expenses)
            {
                if (item.PaidMoney < 0 || item.TotalOneInvoice < 0)
                {
                    return true;
                }
            }

            return false;
        }
        public bool CheckExpenseUpdateNegativeValue(List<ExpenseItemGetDto> expenses)
        {
            foreach (var item in expenses)
            {
                if (item.PaidMoney < 0)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        // Account: Update
        public List<ExpenseItemGetDto> UpdateExpenseAccountDebit(int? companyId, List<ExpenseItemGetDto> incomeItem)
        {
            if (incomeItem == null)
                return null;

            foreach (var item in incomeItem)
            {
                var dbincomeItem = _context.ExpenseItems.Where(w => w.Id == item.Id).ToList();
                foreach (var dbitem in dbincomeItem)
                {
                    //Debit
                    if (dbitem.AccountDebitId == item.AccountDebitId)
                    {
                        //AccountPlan
                        AccountsPlan accountDebit = _context.AccountsPlans.
                        FirstOrDefault(f => f.Id == dbitem.AccountDebitId);
                        if (accountDebit.Debit == null || accountDebit.Debit == 0)
                        {
                            accountDebit.Debit = item.PaidMoney;
                        }
                        else
                        {
                            accountDebit.Debit -= dbitem.PaidMoney;
                            _context.SaveChanges();

                            accountDebit.Debit += item.PaidMoney;
                        }
                        _context.SaveChanges();

                        //Balancsheet

                        BalanceSheet balanceSheetDebit = _context.BalanceSheets.
                        FirstOrDefault(f => f.AccountsPlanId == dbitem.AccountDebitId && f.ExpenseItemId == dbitem.Id);
                        if (balanceSheetDebit != null)
                        {
                            balanceSheetDebit.DebitMoney = item.PaidMoney;
                            balanceSheetDebit.AccountsPlanId = item.AccountDebitId;
                            _context.SaveChanges();
                        }

                        _context.SaveChanges();
                    }
                    else
                    {
                        //AccountPlan
                        AccountsPlan oldAccountDebit = _context.AccountsPlans.
                        FirstOrDefault(f => f.Id == dbitem.AccountDebitId);
                        if (oldAccountDebit.Debit != 0)
                        {
                            oldAccountDebit.Debit -= dbitem.PaidMoney;
                        }
                        _context.SaveChanges();

                        AccountsPlan accountDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == item.AccountDebitId);

                        if (accountDebit.Debit == null || accountDebit.Debit == 0)
                        {
                            accountDebit.Debit = item.PaidMoney;
                        }
                        else
                        {

                            accountDebit.Debit += item.PaidMoney;
                        }
                        _context.SaveChanges();
                        //Balancsheet
                        //remove old balancesheet
                        BalanceSheet oldBalanceSheetDebit = _context.BalanceSheets
                            .FirstOrDefault(f => f.ExpenseItemId == dbitem.Id && f.AccountsPlanId == dbitem.AccountDebitId);
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
                            DebitMoney = item.PaidMoney,
                            AccountsPlanId = item.AccountDebitId,
                            ExpenseItemId = item.Id
                        };
                        _context.BalanceSheets.Add(balanceSheetDebit);
                        _context.SaveChanges();

                    }
                }
            }
            return incomeItem;
        }
        public List<ExpenseItemGetDto> UpdateExpenseAccountKredit(int? companyId, List<ExpenseItemGetDto> incomeItem)
        {
            if (incomeItem == null)
                return null;

            foreach (var item in incomeItem)
            {
                var dbincomeItem = _context.ExpenseItems.Where(w => w.Id == item.Id).ToList();
                foreach (var dbitem in dbincomeItem)
                {
                    //Kredit
                    if (dbitem.AccountKreditId == item.AccountKreditId)
                    {
                        //AccountPlan
                        AccountsPlan accountkredit = _context.AccountsPlans.
                        FirstOrDefault(f => f.Id == dbitem.AccountKreditId);
                        if (accountkredit.Kredit == null || accountkredit.Kredit == 0)
                        {
                            accountkredit.Kredit = item.PaidMoney;
                        }
                        else
                        {
                             accountkredit.Kredit -= dbitem.PaidMoney;
                            _context.SaveChanges();
                          
                            accountkredit.Kredit += item.PaidMoney;
                            _context.SaveChanges();
                        }
                        _context.SaveChanges();
                        //Balancsheet
                        BalanceSheet balanceSheetKredit = _context.BalanceSheets.
                        FirstOrDefault(f => f.AccountsPlanId == dbitem.AccountKreditId && f.ExpenseItemId == dbitem.Id);
                        if (balanceSheetKredit != null)
                        {
                            balanceSheetKredit.KreditMoney = item.PaidMoney;
                            balanceSheetKredit.AccountsPlanId = item.AccountKreditId;
                        }

                        _context.SaveChanges();
                    }
                    else
                    {
                        //AccountPlan
                        AccountsPlan oldAccountKredit = _context.AccountsPlans.
                        FirstOrDefault(f => f.Id == dbitem.AccountKreditId);
                        oldAccountKredit.Kredit -= dbitem.AccountKreditId;

                        _context.SaveChanges();

                        AccountsPlan accountkredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == item.AccountKreditId);
                        if (accountkredit.Kredit == null || accountkredit.Kredit == 0)
                        {
                            accountkredit.Kredit = item.PaidMoney;
                        }
                        else
                        {
                            accountkredit.Kredit += item.PaidMoney;
                        }
                        _context.SaveChanges();
                        //Balancsheet
                        //remove old balancesheet
                        BalanceSheet oldBalanceSheetKredit = _context.BalanceSheets
                            .FirstOrDefault(f => f.ExpenseItemId == dbitem.Id && f.AccountsPlanId == dbitem.AccountKreditId);
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
                            KreditMoney = item.PaidMoney,
                            AccountsPlanId = item.AccountKreditId,
                            ExpenseItemId = dbitem.Id
                        };
                        _context.BalanceSheets.Add(balanceSheetKredit);
                        _context.SaveChanges();
                    }
                }

            }

            return incomeItem;
        }
        //Delete:
        public async Task<ExpenseItem> DeleteExpenseItem(int? expenseItemId)
        {
            if (expenseItemId == null)
                return null;
            // incomeitem
            var expenseItem = await _context.ExpenseItems.Include(i => i.Expense)
             .FirstOrDefaultAsync(f => f.Id == expenseItemId);

            if (expenseItem == null)
                return null;
            //Accounting
            #region Accounting
            var balancesheet = await _context.BalanceSheets.Where(w => w.ExpenseItemId == expenseItemId).ToListAsync();

            var accountPlanDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == expenseItem.AccountDebitId);
            accountPlanDebit.Debit -= expenseItem.PaidMoney;
            _context.SaveChanges();
            var accoutPlanKredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == expenseItem.AccountKreditId);
            accoutPlanKredit.Kredit -= expenseItem.PaidMoney;
            _context.SaveChanges();
            if (balancesheet != null)
            {
                _context.BalanceSheets.RemoveRange(balancesheet);
            }
            #endregion

            //invoice for update IsPaid
            var expenseInvoice = _context.ExpenseInvoices.Find(expenseItem.ExpenseInvoiceId);

            //  deleted  paidmoney sum of residueForCalc   
            if (expenseInvoice.ResidueForCalc != null)
            {
                expenseInvoice.ResidueForCalc += expenseItem.PaidMoney;
            }

            //update invoice status
            if (expenseInvoice.TotalPrice <= expenseInvoice.ResidueForCalc && DateTime.Now > expenseInvoice.EndDate)
            {
                //1=planlinib, 2 = gozlemede, 3=odenilib,4 odenilmeyib
                expenseInvoice.IsPaid = 4;
            }
            else if (expenseInvoice.TotalPrice <= expenseInvoice.ResidueForCalc && DateTime.Now <= expenseInvoice.EndDate)
            {
                expenseInvoice.IsPaid = 1;
            }
            else if (expenseInvoice.TotalPrice > expenseInvoice.ResidueForCalc)
            {
                //1=planlinib, 2 = gozlemede, 3=odenilib
                expenseInvoice.IsPaid = 2;
            }
            else
            {
                //1=planlinib, 2 = gozlemede, 3=odenilib
                expenseInvoice.IsPaid = 3;
            }
            //Deleting Income where equal == deleted incomeitem incomeId And incomeItems Count equal 1
            var expense = _context.Expenses.Include(d => d.ExpenseItems).FirstOrDefault(f => f.Id == expenseItem.ExpenseId);
            if (expense.ExpenseItems.Count() == 1)
            {
                //first deleting incomeItems
                _context.ExpenseItems.Remove(expenseItem);
                await _context.SaveChangesAsync();
                //than deleting income
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();

                return expenseItem;
            }

            //deleting incomeItem without income
            _context.ExpenseItems.Remove(expenseItem);
            await _context.SaveChangesAsync();

            return expenseItem;
        }
        #endregion
    }
}
