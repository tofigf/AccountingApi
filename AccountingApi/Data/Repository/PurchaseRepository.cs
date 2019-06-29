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
        //Checking
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
            var IncomeItems = await _context.ExpenseItems.Where(f => f.ExpenseInvoiceId == invoiceId).ToListAsync();
            // for deleting income
            //there is bug when deleting income ,invoiceId maybe declared difference  
            //    var incomes = _context.Incomes.FirstOrDefault(f => f.Id == f.IncomeItems.FirstOrDefault(a => a.InvoiceId == invoiceId).IncomeId);

            if (IncomeItems.Count > 0)
            {


                foreach (var item in IncomeItems)
                {
                    //removing incomeitems
                    _context.ExpenseItems.Remove(item);

                    _context.SaveChanges();

                }

                ////remove income
                //_context.Incomes.Remove(incomes);
                //_context.SaveChanges();

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


            if (expenseInvoiceItems != null)
            {
                _context.ExpenseInvoiceItems.RemoveRange(expenseInvoiceItems);
            }
            if (expenseItems != null)
            {
                _context.ExpenseItems.RemoveRange(expenseItems);

            }
           
            _context.ExpenseInvoices.Remove(expenseInvoice);

            await _context.SaveChangesAsync();

            return expenseInvoice;

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
                                                   EndDate = inv.EndDate
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
            }
            await _context.SaveChangesAsync();

            return expense;
        }
        //Put
        //so far stopped this method
        public async Task<ExpenseItem> EditExpense(List<ExpenseItem> incomeItems, int? invoiceId)
        {
            //Update ExpenseItems

            double? sumPaidMoney = incomeItems.Sum(s => s.PaidMoney);
            foreach (var item in incomeItems)
            {
                var expenseitem = _context.ExpenseItems.Find(item.Id);

                expenseitem.PaidMoney = item.PaidMoney;
                expenseitem.IsBank = item.IsBank;

                //invoice for update IsPaid
                var invoice = _context.ExpenseInvoices.Find(invoiceId);
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

                if (invoice.ResidueForCalc != null)
                {
                    invoice.ResidueForCalc = invoice.TotalPrice - sumPaidMoney;
                }
            }
            await _context.SaveChangesAsync();

            return null;
        }
        //Check:
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
        public async Task<bool> CheckExpenseEqualingInvoiceTotalPriceForUpdate(List<ExpenseItem> expenseItems, int? invoiceId)
        {
            //total paidmoney
            double? TotalPaidMoney = 0;
            foreach (var item in expenseItems)
            {
                var expenseItemsForPaidMoney = await _context.ExpenseInvoices.FirstOrDefaultAsync(f => f.Id == invoiceId);
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

            if (await _context.Contragents.FirstOrDefaultAsync(a => a.CompanyId == companyId && a.Id != contragentId) == null)
            {
                return true;
            }

            return false;
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
