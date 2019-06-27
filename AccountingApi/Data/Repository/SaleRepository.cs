using AccountingApi.Data.Repository.Interface;
using AccountingApi.Dtos.Sale.Income;
using AccountingApi.Dtos.Sale.Invoice;
using AccountingApi.Dtos.Sale.Proposal;
using AccountingApi.Models;
using EOfficeAPI.Helpers.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DataContext _context;

        public SaleRepository(DataContext context)
        {
            _context = context;
        }

        //Proposal
        #region Proposal
        //Post
        public async Task<Proposal> CreateProposal(Proposal proposal, int? companyId)
        {
            if (companyId == null)
                return null;
            if (proposal != null)
            {
                proposal.CreatedAt = DateTime.UtcNow.AddHours(4);
                //1=planlinib, 2 = gozlemede,3=odenilib
                //proposal.IsPaid = 1;
                proposal.CompanyId = Convert.ToInt32(companyId);
                await _context.Proposals.AddAsync(proposal);
                await _context.SaveChangesAsync();
            }

            return proposal;
        }
        public async Task<IEnumerable<ProposalItem>> CreateProposalItems(IEnumerable<ProposalItem> items, int? proposalId)
        {
            foreach (var item in items)
            {
                item.ProposalId = proposalId;
                //item.TotalOneProduct = item.Qty * item.SellPrice;

                await _context.ProposalItems.AddAsync(item);
                await _context.SaveChangesAsync();
            }

            return items;
        }
        //Email
        public ProposalSentMail CreateProposalSentMail(int? proposalId, string email)
        {
            ProposalSentMail proposalSent = new ProposalSentMail
            {
                ProposalId = Convert.ToInt32(proposalId),
                Email = email,
                Token = CryptoHelper.Crypto.HashPassword(DateTime.Now.ToLongDateString()).Replace('+', 't'),

                //1=planlinib, 2 = gozlemede,3=odenilib
                IsPaid = 2
            };

            _context.ProposalSentMails.Add(proposalSent);
            _context.SaveChanges();

            return proposalSent;
        }
        public Proposal GetProposalByToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            Proposal proposal = _context.ProposalSentMails.Include(a => a.Proposal)
                .ThenInclude(t => t.ProposalItems).ThenInclude(a => a.Product).FirstOrDefault(f => f.Token == token).Proposal;





            return proposal;
        }
        //Update zamani bu metodla dolduracayiq
        public async Task<Company> GetEditCompany(int? companyId)
        {
            if (companyId == null)
                return null;
            Company company = await _context.Companies.FindAsync(companyId);

            return company;
        }
        public async Task<Company> EditCompany(Company company, int? companyId, int? userId)
        {
            if (company == null)
                return null;
            if (companyId == null)
                return null;
            if (userId == null)
                return null;
            _context.Entry(company).State = EntityState.Modified;
            _context.Entry(company).Property(a => a.UserId).IsModified = false;

            await _context.SaveChangesAsync();

            return company;
        }
        public async Task<Contragent> GetEditContragent(int? contragentId)
        {
            if (contragentId == null)
                return null;

            Contragent contragent = await _context.Contragents.FindAsync(contragentId);

            return contragent;
        }
        public async Task<Contragent> EditContragent(Contragent contragent, int? companyId)
        {
            //update entitynin metodudu.
            _context.Entry(contragent).State = EntityState.Modified;
            _context.Entry(contragent).Property(a => a.CompanyId).IsModified = false;

            //Commit the transaction
            await _context.SaveChangesAsync();

            return contragent;
        }
        //Get: Proposal
        public async Task<List<ProposalGetDto>> GetProposal(PaginationParam productParam, int? companyId)
        {
            if (companyId == null)
                return null;
            List<Proposal> proposals = await _context.Proposals.Where(w => w.CompanyId == companyId)
            .OrderByDescending(d => d.Id).ToListAsync();
            List<Contragent> contragents = await _context.Contragents
        .OrderByDescending(d => d.Id).ToListAsync();

            var proposalcon = proposals.Join(contragents, m => m.ContragentId, m => m.Id, (pro, con) => new ProposalGetDto
            {
                ContragentCompanyName = con.CompanyName,
                ProposalNumber = pro.ProposalNumber,
                TotalPrice = pro.TotalPrice,
                PreparingDate = pro.PreparingDate,
                EndDate = pro.EndDate,
                //IsPaid =pro.IsPaid,
                Id = pro.Id
            }).ToList();

            return proposalcon;
        }
        //get edit details
        public async Task<Proposal> GetDetailProposal(int? proposalId, int? companyId)
        {
            if (proposalId == null)
                return null;
            if (companyId == null)
                return null;

            Proposal proposal = await _context.Proposals.Include(i => i.Company).Include(i => i.Tax).Include(i => i.ProposalItems).ThenInclude(a => a.Product).FirstOrDefaultAsync(f => f.Id == proposalId && f.CompanyId == companyId);
            if (proposal == null)
                return null;

            return proposal;
        }
        //get edit proposal
        public async Task<Proposal> GetEditProposal(int? proposalId, int? companyId)
        {
            if (proposalId == null)
                return null;

            Proposal proposal = await _context.Proposals.FirstOrDefaultAsync(f => f.Id == proposalId && f.CompanyId == companyId);

            if (proposal == null)
                return null;

            return proposal;
        }
        public async Task<Contragent> GetContragentProposal(int? companyId, int? proposalId)
        {
            Contragent contragent = await _context.Contragents.SingleOrDefaultAsync(c => c.CompanyId == companyId &&

           c.Proposals.SingleOrDefault(w => w.Id == proposalId) != null);
            if (contragent == null)
                return null;
            if (contragent == null)
                return null;
            return contragent;

        }

        // get edit proposalitems
        public async Task<List<ProposalItem>> GetEditProposalItem(int? proposalId)
        {
            if (proposalId == null)
                return null;
            List<ProposalItem> proposalItems = await _context.ProposalItems.Where(w => w.ProposalId == proposalId).AsNoTracking().ToListAsync();

            if (proposalItems == null)
                return null;
            return proposalItems;


        }
        //Check
        #region Check
        public async Task<bool> CheckProposal(int? currentUserId, int? companyId)
        {
            if (currentUserId == null)
                return true;
            if (companyId == null)
                return true;
            if (await _context.Proposals.AnyAsync(a => a.CompanyId == companyId && a.Company.UserId != currentUserId))
                return true;

            return false;
        }
        // mehsulun id-sinin olmasini yoxlayiriq.
        public async Task<bool> CheckProposalProductId(IEnumerable<ProposalItem> proposalItem)
        {
            foreach (var p in proposalItem)
            {
                if (await _context.Products.FirstOrDefaultAsync(a => a.Id == p.ProductId) == null)
                {
                    return true;
                }
            }

            return false;
        }
        public async Task<bool> CheckProposalId(int? productId, int? companyId)
        {
            if (companyId == null)
                return true;
            if (productId == null)
                return true;
            if (await _context.Proposals.AnyAsync(a => a.CompanyId != companyId && a.Id == productId))
                return true;

            return false;
        }

        public async Task<bool> CheckContragentId(int? contragentId, int? companyId)
        {
            if (companyId == null)
                return true;
            if (contragentId == null)
                return true;

            if (await _context.Contragents.FirstOrDefaultAsync(a => a.CompanyId == companyId && a.Id == contragentId) == null)
                return true;

            return false;
        }
        #endregion
        //Put
        public async Task<Proposal> EditProposal(Proposal proposal, List<ProposalItem> proposalItems, int? proposalId)
        {
            if (proposalId == null)
                return null;
            if (proposal == null)
                return null;
            if (proposalItems == null)
                return null;

            //update proposal
            _context.Entry(proposal).State = EntityState.Modified;
            _context.Entry(proposal).Property(a => a.CompanyId).IsModified = false;

            await _context.SaveChangesAsync();

            //Update IncomeItems
            foreach (var item in proposalItems.Where(w => w.Id != 0))
            {
                _context.Entry(item).State = EntityState.Modified;

                _context.Entry(item).Property(a => a.ProposalId).IsModified = false;
            }
            _context.SaveChanges();
            foreach (var inv in proposalItems.Where(w => w.Id == 0))
            {
                ProposalItem proposalItem = new ProposalItem
                {
                    Qty = inv.Qty,
                    Price = inv.Price,
                    SellPrice = inv.SellPrice,
                    TotalOneProduct = inv.TotalOneProduct,
                    ProductId = inv.ProductId,
                    ProposalId = Convert.ToInt32(proposalId)
                };
                _context.ProposalItems.Add(proposalItem);
            }
            _context.SaveChanges();
            return proposal;
        }
        //Delete DeleteProposalItem
        public async Task<ProposalItem> DeleteProposalItem(int? proposalItemId)
        {
            if (proposalItemId == null)
                return null;
            //ProposalItems
            var proposalItem = await _context.ProposalItems.Include(i => i.Proposal).FirstOrDefaultAsync(f => f.Id == proposalItemId);

            if (proposalItem == null)
                return null;
            //Proposal
            var proposal = _context.Proposals.Include(i => i.Tax).FirstOrDefault(f => f.Id == proposalItem.Proposal.Id);
            //New Proposal Sum value
            proposal.Sum -= proposalItem.SellPrice * proposalItem.Qty;
            //New Proposal TotalTax value
            proposal.TotalTax = proposal.Sum * proposal.Tax.Rate / 100;
            //New Proposal TotalPrice value
            proposal.TotalPrice = proposal.Sum + proposal.TotalTax;

            await _context.SaveChangesAsync();
            //Remove PoposalItems
            _context.ProposalItems.Remove(proposalItem);

            await _context.SaveChangesAsync();

            return proposalItem;
        }
        //Delete Proposal 
        public async Task<Proposal> DeleteProposal(int? companyId, int? proposalId)
        {
            if (companyId == null)
                return null;
            if (proposalId == null)
                return null;

            var proposal = await _context.Proposals.FirstOrDefaultAsync(f => f.Id == proposalId && f.CompanyId == companyId);
            if (proposal == null)
                return null;
            var proposalItems = await _context.ProposalItems.Where(w => w.ProposalId == proposalId).ToListAsync();

            var proposalSentMails = await _context.ProposalSentMails.Where(w => w.ProposalId == proposalId).ToListAsync();

            if (proposalItems != null)
            {
                _context.ProposalItems.RemoveRange(proposalItems);
            }
            if (proposalSentMails != null)
            {
                _context.ProposalSentMails.RemoveRange(proposalSentMails);

            }

            _context.Proposals.Remove(proposal);

            await _context.SaveChangesAsync();

            return proposal;

        }
        #endregion

        //Invoice
        #region Invoice
        //Post
        public async Task<Invoice> CreateInvoice(Invoice invoice, int? companyId)
        {
            if (companyId == null)
                return null;
            if (invoice.ContragentId == null)
                return null;

            invoice.CreatedAt = DateTime.UtcNow.AddHours(4);
            invoice.ResidueForCalc = invoice.TotalPrice;

            if (invoice == null)
                return null;

            if (DateTime.Now > invoice.EndDate && invoice.TotalPrice == invoice.ResidueForCalc)
            {
                invoice.IsPaid = 4;
            }

            
            //1=planlinib, 2 = gozlemede,3=odenilib
            //invoice.IsPaid = 1;
            invoice.CompanyId = Convert.ToInt32(companyId);
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            //AccountPlan
            AccountsPlan accountDebit = await _context.AccountsPlans.FirstOrDefaultAsync(f => f.Id == invoice.AccountDebitId);
            accountDebit.Debit = invoice.TotalPrice;
            AccountsPlan accountkredit = await _context.AccountsPlans.FirstOrDefaultAsync(f => f.Id == invoice.AccountKreditId);
            accountkredit.Kredit = invoice.TotalPrice;
            BalanceSheet balanceSheetDebit = new BalanceSheet
            {
                CreatedAt = DateTime.Now,
                CompanyId = Convert.ToInt32(companyId),
                DebitMoney = invoice.TotalPrice,
                AccountsPlanId = invoice.AccountDebitId,
                InvoiceId = invoice.Id
            };
            await _context.BalanceSheets.AddAsync(balanceSheetDebit);
            BalanceSheet balanceSheetKredit = new BalanceSheet
            {
                CreatedAt = DateTime.Now,
                CompanyId = Convert.ToInt32(companyId),
                KreditMoney = invoice.TotalPrice,
                AccountsPlanId = invoice.AccountKreditId,
                InvoiceId = invoice.Id
            };
            await _context.BalanceSheets.AddAsync(balanceSheetKredit);
            await _context.SaveChangesAsync();

            return invoice;
        }
        public async Task<List<InvoiceItem>> CreateInvoiceItems(List<InvoiceItem> items, int? invoiceId)
        {
            if (invoiceId == null)
                return null;

            foreach (var item in items)
            {

                item.InvoiceId = invoiceId;

                await _context.InvoiceItems.AddAsync(item);
                await _context.SaveChangesAsync();
            }

            return items;
        }
        //Get
        //get all invoice
        public async Task<List<InvoiceGetDto>> GetInvoice(PaginationParam productParam, int? companyId)
        {
            if (companyId == null)
                return null;
            List<Invoice> invoices = await _context.Invoices.Where(w => w.CompanyId == companyId)
            .OrderByDescending(d => d.Id).ToListAsync();
            List<Contragent> contragents = await _context.Contragents
        .OrderByDescending(d => d.Id).ToListAsync();

            var proposalcon = invoices.Join(contragents, m => m.ContragentId, m => m.Id, (inv, con) => new InvoiceGetDto
            {
                ContragentCompanyName = con.CompanyName,
                InvoiceNumber = inv.InvoiceNumber,
                TotalPrice = inv.TotalPrice,
                PreparingDate = inv.PreparingDate,
                EndDate = inv.EndDate,
                IsPaid = inv.IsPaid,
                Id = inv.Id
            }).ToList();

            return proposalcon;
        }
        // detail used in get edit invoice 
        public async Task<Invoice> GetDetailInvoice(int? invoiceId, int? companyId)
        {
            if (invoiceId == null)
                return null;
            if (companyId == null)
                return null;

            Invoice invoice = await _context.Invoices.Include(i => i.Company)
                .Include(i => i.Tax).Include(i => i.InvoiceItems)
                .ThenInclude(a => a.Product).Include(i => i.AccountsPlanDebit)
                .Include(t => t.AccountsPlanKredit)
                .FirstOrDefaultAsync(f => f.Id == invoiceId && f.CompanyId == companyId);

            return invoice;
        }
        //used in update action
        public async Task<Invoice> GetEditInvoice(int? invoiceId, int? companyId)
        {
            if (invoiceId == null)
                return null;
            if (companyId == null)
                return null;

            Invoice invoice = await _context.Invoices.Include(i=>i.AccountsPlanDebit).Include(t=>t.AccountsPlanKredit)
                .FirstOrDefaultAsync(f => f.Id == invoiceId && f.CompanyId == companyId);

            return invoice;
        }
        //used in update action
        public async Task<List<InvoiceItem>> GetEditInvoiceItem(int? invoiceId)
        {
            if (invoiceId == null)
                return null;

            List<InvoiceItem> invoiceItems = await _context.InvoiceItems.Where(w => w.InvoiceId == invoiceId).AsNoTracking().ToListAsync();

            return invoiceItems;
        }
        //get contragent by invoiceid
        public async Task<Contragent> GetContragentInvoice(int? companyId, int? invoiceId)
        {

            Contragent contragent = await _context.Contragents.SingleOrDefaultAsync(c => c.CompanyId == companyId &&

          c.Invoices.SingleOrDefault(w => w.Id == invoiceId) != null);

            return contragent;
        }
        //Put:
     // Accounting Update
        public InvoicePutDto UpdateAccountDebit(int? invoiceId,int? companyId, InvoicePutDto invoice, int? OldDebitId)
        {
            if (invoiceId == null)
                return null;

            if (invoice == null)
                return null;
           
            //Debit
            if (OldDebitId == invoice.AccountDebitId)
            {
                //AccountPlan
                AccountsPlan accountDebit =  _context.AccountsPlans.FirstOrDefault(f => f.Id == OldDebitId);
                accountDebit.Debit = invoice.TotalPrice;
                _context.SaveChanges();
                //Balancsheet
                BalanceSheet balanceSheetDebit =  _context.BalanceSheets.FirstOrDefault(f => f.AccountsPlanId == OldDebitId);
                balanceSheetDebit.DebitMoney = invoice.TotalPrice;
                balanceSheetDebit.AccountsPlanId = invoice.AccountDebitId;
                _context.SaveChanges();
            }
            else
            {
                //AccountPlan
                AccountsPlan oldAccountDebit =   _context.AccountsPlans.FirstOrDefault(f => f.Id == OldDebitId);
                oldAccountDebit.Debit = 0.00;
                _context.SaveChanges();
                AccountsPlan accountDebit =   _context.AccountsPlans.FirstOrDefault(f => f.Id == invoice.AccountDebitId);
                accountDebit.Debit = invoice.TotalPrice;
                _context.SaveChanges();
                //Balancsheet
                //remove old balancesheet
                BalanceSheet oldBalanceSheetDebit =  _context.BalanceSheets
                    .FirstOrDefault(f => f.InvoiceId == invoiceId && f.AccountsPlanId == OldDebitId);
                if(oldBalanceSheetDebit != null)
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
                    InvoiceId = invoiceId
                };
                 _context.BalanceSheets.Add(balanceSheetDebit);
                _context.SaveChanges();

            }
  

            return invoice;
        }
        public InvoicePutDto UpdateAccountKredit(int? invoiceId, int? companyId, InvoicePutDto invoice, int? OldKeditId)
        {
            if (invoiceId == null)
                return null;

            if (invoice == null)
                return null;
            //Kredit
            if (OldKeditId == invoice.AccountKreditId)
            {
                //AccountPlann
                AccountsPlan accountkredit =  _context.AccountsPlans.FirstOrDefault(f => f.Id == OldKeditId);
                accountkredit.Kredit = invoice.TotalPrice;
                 _context.SaveChanges();
                //Balancsheet
                BalanceSheet balanceSheetKredit =  _context.BalanceSheets.FirstOrDefault(f => f.AccountsPlanId == OldKeditId);
                balanceSheetKredit.KreditMoney = invoice.TotalPrice;
                balanceSheetKredit.AccountsPlanId = invoice.AccountKreditId;
                _context.SaveChanges();
            }
            else
            {
                //AccountPlan
                AccountsPlan oldAccountKredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == OldKeditId);
                oldAccountKredit.Kredit = 0.00;
                _context.SaveChanges();
                AccountsPlan accountKredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == invoice.AccountKreditId);
                accountKredit.Kredit = invoice.TotalPrice;
                _context.SaveChanges();
                //Balancsheet
                //remove old balancesheet
                BalanceSheet oldBalanceSheetKredit =  _context.BalanceSheets
                    .FirstOrDefault(f => f.InvoiceId == invoiceId && f.AccountsPlanId == OldKeditId);
                if(oldBalanceSheetKredit != null)
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
                    InvoiceId = invoiceId
                };
                _context.BalanceSheets.Add(balanceSheetKredit);
                _context.SaveChanges();
            }

            return invoice;
        }
        public async Task<Invoice> EditInvoice(Invoice invoice, List<InvoiceItem> invoiceItems, int? invoiceId)
        {

            if (invoiceId == null)
                return null;

            if (invoice == null)
                return null;
           

            var invoiceforpaidmoney = _context.Invoices.Find(invoice.Id);
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
           await _context.SaveChangesAsync();
            //update invoice
            _context.Entry(invoice).State = EntityState.Modified;
            _context.Entry(invoice).Property(a => a.CompanyId).IsModified = false;
            //_context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();

            //Update IncomeItems
            foreach (var item in invoiceItems.Where(w => w.Id != 0))
            {

                //InvoiceItem dbInvoiceItem = await _context.InvoiceItems
                //     .FirstOrDefaultAsync(w => w.InvoiceId == invoiceId && w.Id == item.Id);
                ////Controller de yoxlamaq lazimdi error qaytarmaq lazimdi eger invocittem id-si  db yoxdusa
                //if(dbInvoiceItem == null)
                //{
                //    return null;
                //}

                _context.Entry(item).State = EntityState.Modified;
                _context.Entry(item).Property(a => a.InvoiceId).IsModified = false;
            }
            await _context.SaveChangesAsync();
            foreach (var inv in invoiceItems.Where(w => w.Id == 0))
            {
                InvoiceItem invoiceItem = new InvoiceItem
                {

                    Qty = inv.Qty,
                    Price = inv.Price,
                    SellPrice = inv.SellPrice,
                    TotalOneProduct = inv.TotalOneProduct,
                    ProductId = inv.ProductId,
                    InvoiceId = Convert.ToInt32(invoiceId)
                };
                _context.InvoiceItems.Add(invoiceItem);
            }
            await _context.SaveChangesAsync();

            // find invoiceById for equal totaprice to resdueForCalc.. because of correct calculating
            var foundinvoice = await _context.Invoices.FindAsync(invoiceId);
            // for equal
            foundinvoice.ResidueForCalc = foundinvoice.TotalPrice;

            await _context.SaveChangesAsync();

            // for deleting incomesitems
            //var IncomeItems = await _context.IncomeItems.Where(f => f.InvoiceId == invoiceId).ToListAsync();
            // for deleting income
            //there is bug when deleting income ,invoiceId maybe declared difference  
            //    var incomes = _context.Incomes.FirstOrDefault(f => f.Id == f.IncomeItems.FirstOrDefault(a => a.InvoiceId == invoiceId).IncomeId);

            //if (IncomeItems.Count > 0)
            //{


            //    foreach (var item in IncomeItems)
            //    {
            //        //removing incomeitems
            //        _context.IncomeItems.Remove(item);

            //        _context.SaveChanges();

            //    }
            ////remove income
            //_context.Incomes.Remove(incomes);
            //_context.SaveChanges();

            //   }

            return invoice;

        }
        //Check:
        #region Check
        public async Task<bool> CheckInvoice(int? currentUserId, int? companyId)
        {
            if (currentUserId == null)
                return true;
            if (companyId == null)
                return true;
            if (await _context.Invoices.AnyAsync(a => a.CompanyId == companyId && a.Company.UserId != currentUserId))
                return true;

            return false;
        }
      

            public async Task<bool> CheckInvoiceProductId(List<InvoiceItem> invoiceItems)
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
        public async Task<bool> CheckInvoiceId(int? productId, int? companyId)
        {
            if (companyId == null)
                return true;
            if (productId == null)
                return true;
            if (await _context.Invoices.AnyAsync(a => a.CompanyId != companyId && a.Id == productId))
                return true;

            return false;
        }
        public async Task<bool> CheckInvoiceItem(int? invoiceId, List<InvoiceItem> invoiceItems)
        {
            foreach (var item in invoiceItems.Where(w => w.Id != 0))
            {
                InvoiceItem dbInvoiceItem = await _context.InvoiceItems.AsNoTracking()
                     .FirstOrDefaultAsync(w => w.InvoiceId == invoiceId && w.Id == item.Id);

                if (dbInvoiceItem == null)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        //checking exist income
        //public async Task<bool> CheckExistIncomeByInvoiceId(int? invoiceId)
        //{

        //    var existIncome = await _context.IncomeItems.FirstOrDefaultAsync(f => f.InvoiceId == invoiceId);
        //    if (existIncome != null)
        //        return true;

        //    return false;
        //}
        //Delete:DeleteInvoiceItem
        public async Task<InvoiceItem> DeleteInvoiceItem(int? invoiceItemId)
        {
            if (invoiceItemId == null)
                return null;
            //InvoicesItems
            var invoiceItem = await _context.InvoiceItems.Include(i => i.Invoice).FirstOrDefaultAsync(f => f.Id == invoiceItemId);

            if (invoiceItem == null)
                return null;
            //Invoice
            var invoice = _context.Invoices.Include(t => t.Tax).FirstOrDefault(f => f.Id == invoiceItem.Invoice.Id);
            //New Invoice Sum value
            invoice.Sum -= (invoiceItem.SellPrice * invoiceItem.Qty);
            //New Invoice TotalTax value
            invoice.TotalTax = invoice.Sum * invoice.Tax.Rate / 100;
            //New Invoice TotalPrice value
            invoice.TotalPrice = invoice.Sum + invoice.TotalTax;
            //New Invoice ResidueForCalc value
            invoice.ResidueForCalc = invoice.Sum + invoice.TotalTax;

            _context.SaveChanges();
            //Accounting
            #region Accounting
            var balancesheetDebit =  _context.BalanceSheets.FirstOrDefault(w =>w.InvoiceId == invoice.Id && w.AccountsPlanId == invoice.AccountDebitId);
            balancesheetDebit.DebitMoney = invoice.TotalPrice;
            _context.SaveChanges();
            var balancesheetKredit = _context.BalanceSheets.FirstOrDefault(w => w.InvoiceId == invoice.Id && w.AccountsPlanId == invoice.AccountKreditId);
            balancesheetKredit.KreditMoney = invoice.TotalPrice;
            _context.SaveChanges();
            
            var accountPlanDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == invoice.AccountDebitId);
            accountPlanDebit.Debit = invoice.TotalPrice;
            _context.SaveChanges();
            var accoutPlanKredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == invoice.AccountKreditId);
            accoutPlanKredit.Kredit = invoice.TotalPrice;
            _context.SaveChanges();
            #endregion
            //Remove InvoiceItems
            _context.InvoiceItems.Remove(invoiceItem);

            await _context.SaveChangesAsync();

            return invoiceItem;
        }
        //Delete:DeleteInvoice
        public async Task<Invoice> DeleteInvoice(int? companyId, int? invoiceId)
        {
            if (companyId == null)
                return null;
            if (invoiceId == null)
                return null;

            var invoice = await _context.Invoices.FirstOrDefaultAsync(f => f.Id == invoiceId && f.CompanyId == companyId);
            if (invoice == null)
                return null;

            var invoiceItems = await _context.InvoiceItems.Where(w => w.InvoiceId == invoiceId).ToListAsync();

            //var incomeItems = await _context.IncomeItems.Where(w => w.InvoiceId == invoiceId).ToListAsync();

            var invoiceSentMails = await _context.InvoiceSentMails.Where(w => w.InvoiceId == invoiceId).ToListAsync();

            //Accounting
            #region Accounting
            var balancesheet = await _context.BalanceSheets.Where(w => w.CompanyId == companyId && w.InvoiceId == invoiceId).ToListAsync();

            var accountPlanDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == invoice.AccountDebitId);
            accountPlanDebit.Debit -= invoice.TotalPrice;
            _context.SaveChanges();
            var accoutPlanKredit = _context.AccountsPlans.FirstOrDefault(f => f.Id == invoice.AccountKreditId);
            accoutPlanKredit.Kredit -= invoice.TotalPrice;
            _context.SaveChanges();
            #endregion

            if (invoiceItems != null)
            {
                _context.InvoiceItems.RemoveRange(invoiceItems);
            }
            //if (incomeItems != null)
            //{
            //    _context.IncomeItems.RemoveRange(incomeItems);

            //}
            if (invoiceSentMails != null)
            {
                _context.InvoiceSentMails.RemoveRange(invoiceSentMails);
            }
            if(balancesheet != null)
            {
                _context.BalanceSheets.RemoveRange(balancesheet);
            }
            _context.Invoices.Remove(invoice);



            await _context.SaveChangesAsync();

            return invoice;

        }

        //Email
        public InvoiceSentMail CreateInvoiceSentMail(int? invoiceId, string email)
        {
            InvoiceSentMail invoiceSent = new InvoiceSentMail
            {
                InvoiceId = Convert.ToInt32(invoiceId),
                Email = email,
                Token = CryptoHelper.Crypto.HashPassword(DateTime.Now.ToLongDateString()).Replace('+', 't'),

                //1=planlinib, 2 = gozlemede,3=odenilib
                IsPaid = 2
            };

            _context.InvoiceSentMails.Add(invoiceSent);
            _context.SaveChanges();

            return invoiceSent;
        }
        public Invoice GetInvoiceByToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            Invoice invoice = _context.InvoiceSentMails.Include(a => a.Invoice)
                .ThenInclude(t => t.InvoiceItems).ThenInclude(a => a.Product).FirstOrDefault(f => f.Token == token).Invoice;

            return invoice;
        }

        #endregion

        //Income
        #region InCome
        //Get 
        public List<IncomeInvoiceGetDto> GetInvoiceByContragentId(int? contragentId, int? companyId)
        {
            if (contragentId == null)
                return null;
            List<IncomeInvoiceGetDto> datas = (from inv in _context.Invoices
                                                   //join itemincome in _context.IncomeItems
                                                   //on inv.Id equals itemincome.InvoiceId
                                                   //Left join for take residue in table income
                                                   //into sr
                                                   //from x in sr.DefaultIfEmpty()
                                               where inv.ContragentId == contragentId && inv.CompanyId == companyId && inv.IsPaid != 3
                                               select new IncomeInvoiceGetDto
                                               {
                                                   InvoiceId = inv.Id,
                                                   InvoiceNumber = inv.InvoiceNumber,
                                                   TotalPrice = inv.TotalPrice,
                                                   Residue = inv.ResidueForCalc,
                                                   PreparingDate = inv.PreparingDate,
                                                   EndDate = inv.EndDate
                                               }).ToList();
            return datas;
        }
        public async Task<List<IncomeGetDto>> GetIncome(PaginationParam productParam, int? companyId)
        {
            if (companyId == null)
                return null;

            var incomeItems = await _context.IncomeItems

                    .Include(i => i.Income)
                    .Include(a => a.Invoice)
                    .ThenInclude(t => t.Contragent)
                    .Where(w => w.Income.CompanyId == companyId)
                    .OrderByDescending(d => d.Id)
                    .GroupBy(p => p.InvoiceId)
                    .Select(g => new
                    {
                        first = g.First(),
                        sum = g.Sum(s => s.PaidMoney)
                    }

                    ).ToListAsync();

            var joinIncome = incomeItems.Select(s => new IncomeGetDto
            {
                ContragentCompanyName = s.first.Income.Contragent.CompanyName,
                ContragentFullname = s.first.Income.Contragent.Fullname,
                InvoiceNumber = s.first.InvoiceNumber,
                Id = s.first.Income.Id,
                IsBank = s.first.IsBank,
                TotalPrice = s.first.Income.TotalPrice,
                PaidMoney = s.first.PaidMoney,
                Residue = s.first.Invoice.ResidueForCalc,
                CreatedAt = s.first.Income.CreatedAt,
                //income page mebleg
                TotalOneInvoice = s.first.TotalOneInvoice,
                InvoiceId = s.first.InvoiceId,
                SumPaidMoney = s.sum
            }).ToList();

            return joinIncome;
        }
        //get for editing income
        public async Task<Income> GetEditIncome(int? incomeId, int? companyId)
        {
            if (incomeId == null)
                return null;
            if (companyId == null)
                return null;

            Income income = await _context.Incomes.FirstOrDefaultAsync(f => f.CompanyId == companyId && f.Id == incomeId);

            return income;
        }
        public async Task<List<IncomeItem>> GetEditIncomeItems(int? invoiceId)
        {
            if (invoiceId == null)
                return null;

            List<IncomeItem> incomeItems = await _context.IncomeItems.Include(i=>i.AccountsPlanDebit).Include(s=>s.AccountsPlanKredit)
                .Where(w => w.InvoiceId == invoiceId).AsNoTracking().ToListAsync();

            return incomeItems;
        }

        //get single income invoices didnt use
        public async Task<List<IncomeItem>> GetEditAllIncomes(int? companyId, int? invoiceId)
        {
            if (invoiceId == null)
                return null;
            if (companyId == null)
                return null;

            var incomeItems = await _context.IncomeItems.Include(i => i.Income)
                  .Where(f => f.Invoice.CompanyId == companyId && f.InvoiceId == invoiceId).ToListAsync();

            var test = _context.Invoices.Include(i => i.IncomeItems).ThenInclude(t => t.Income).FirstOrDefaultAsync(f => f.Id == invoiceId);

            return incomeItems;
        }
        //https://localhost:44317/api/income/geteditincome
        //get edit income
        public Task<Invoice> GetInvoiceIcomeItem(int? companyId, int? invoiceId)
        {
            if (invoiceId == null)
                return null;
            if (companyId == null)
                return null;

            var invoice = _context.Invoices.Include(i => i.IncomeItems).ThenInclude(t => t.Income).Include(c => c.Contragent)
                .FirstOrDefaultAsync(f => f.CompanyId == companyId && f.Id == invoiceId);
            if (invoice == null)
                return null;

            return invoice;
        }
        //get single income detail
        public async Task<Income> DetailIncome(int? incomeId, int? companyId)
        {
            if (incomeId == null)
                return null;

            Income income = await _context.Incomes.Include(i => i.Contragent).Include(i => i.IncomeItems).FirstOrDefaultAsync(f => f.CompanyId == companyId && f.Id == incomeId);

            return income;
        }
        //Post
        public async Task<Income> CreateIncome(int? companyId, int? contragentId, int[] Ids, Income income, List<IncomeItem> incomes)
        {
            if (companyId == null)
                return null;

            if (contragentId == null)
                return null;

            income.CreatedAt = DateTime.UtcNow.AddHours(4);
            income.CompanyId = Convert.ToInt32(companyId);
            income.ContragentId = Convert.ToInt32(contragentId);
            await _context.Incomes.AddAsync(income);
            await _context.SaveChangesAsync();

            //foreach (var id in Ids)
            //{
            foreach (var inc in incomes)
            {
                //invoice for update IsPaid
                var invoice = _context.Invoices.Find(inc.InvoiceId);

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

                inc.IncomeId = income.Id;

                await _context.IncomeItems.AddAsync(inc);

                // }

                //AccountPlan
                AccountsPlan accountDebit = _context.AccountsPlans.FirstOrDefault(f => f.Id == inc.AccountDebitId);
                accountDebit.Debit = inc.PaidMoney;
                _context.SaveChanges();
                AccountsPlan accountkredit =  _context.AccountsPlans.FirstOrDefault(f => f.Id == inc.AccountKreditId);
                accountkredit.Kredit = inc.PaidMoney;
                _context.SaveChanges();
                BalanceSheet balanceSheetDebit = new BalanceSheet
                {
                    CreatedAt = DateTime.Now,
                    CompanyId = Convert.ToInt32(companyId),
                    DebitMoney = inc.PaidMoney,
                    AccountsPlanId = inc.AccountDebitId,
                    IncomeItemId = inc.Id
                };
                _context.BalanceSheets.Add(balanceSheetDebit);
                _context.SaveChanges();
                BalanceSheet balanceSheetKredit = new BalanceSheet
                {
                    CreatedAt = DateTime.Now,
                    CompanyId = Convert.ToInt32(companyId),
                    KreditMoney = inc.PaidMoney,
                    AccountsPlanId = inc.AccountKreditId,
                    IncomeItemId = inc.Id
                };
                 _context.BalanceSheets.Add(balanceSheetKredit);
                _context.SaveChanges();
            }
            await _context.SaveChangesAsync();

            return income;
        }
        //Check
        public async Task<bool> CheckIncome(int? currentUserId, int? companyId)
        {
            if (currentUserId == null)
                return true;
            if (companyId == null)
                return true;
            if (await _context.Incomes.AnyAsync(a => a.CompanyId == companyId && a.Company.UserId != currentUserId))
                return true;

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
        //check invoice total price with paidmoney
        public async Task<bool> CheckIncomeEqualingInvoiceTotalPriceForUpdate(List<IncomeItem> incomeItems, int? invoiceId)
        {
            //total paidmoney
            double? TotalPaidMoney = 0;
            foreach (var item in incomeItems)
            {
                var incomeItemsForPaidMoney = await _context.Invoices.FirstOrDefaultAsync(f => f.Id == invoiceId);
                if (incomeItemsForPaidMoney == null)
                    return true;
                TotalPaidMoney += item.PaidMoney;

                //checkig totalpaidmoney and totaloneinvoice
                if (incomeItemsForPaidMoney.TotalPrice < TotalPaidMoney)
                {
                    return true;
                }
                if (_context.IncomeItems.FirstOrDefault(f => f.Id == item.Id) == null)
                {
                    return true;
                }
            }

            return false;
        }
        public async Task<bool> CheckIncomeEqualingInvoiceTotalPriceForCreate(List<IncomeItem> incomeItems)
        {
            foreach (var item in incomeItems)
            {
                var incomeItemsForPaidMoney = await _context.Invoices.Where(f => f.Id == item.InvoiceId).ToListAsync();
                if (incomeItemsForPaidMoney == null)
                    return true;
                //total paidmoney
                double? TotalPaidMoney = 0;

                foreach (var incpaid in incomeItemsForPaidMoney)
                {

                    TotalPaidMoney += item.PaidMoney;

                    //checkig totalpaidmoney and totaloneinvoice
                    if (incpaid.TotalPrice < TotalPaidMoney)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        //Put
        //so far stopped this method
        public async Task<IncomeItem> EditIncome(List<IncomeItem> incomeItems, int? invoiceId)
        {
            //Update IncomeItems

            double? sumPaidMoney = incomeItems.Sum(s => s.PaidMoney);
            foreach (var item in incomeItems)
            {
                var invitem = _context.IncomeItems.Find(item.Id);

                invitem.PaidMoney = item.PaidMoney;
                invitem.IsBank = item.IsBank;

                //invoice for update IsPaid
                var invoice = _context.Invoices.Find(invoiceId);
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
        //Delete
        public async Task<IncomeItem> DeleteIncomeItem(int? incomeItemId)
        {
            if (incomeItemId == null)
                return null;
            // incomeitem
            var incomeItem = await _context.IncomeItems.Include(i => i.Invoice)
             .FirstOrDefaultAsync(f => f.Id == incomeItemId);

            if (incomeItem == null)
                return null;

            //invoice for update IsPaid
            var invoice = _context.Invoices.Find(incomeItem.InvoiceId);

            //  deleted  paidmoney sum of residueForCalc   
            if (invoice.ResidueForCalc != null)
            {
                invoice.ResidueForCalc += incomeItem.PaidMoney;
            }

            //update invoice status

            if (invoice.TotalPrice <= invoice.ResidueForCalc && DateTime.Now > invoice.EndDate)
            {
                //1=planlinib, 2 = gozlemede, 3=odenilib,4 odenilmeyib
                invoice.IsPaid = 4;
            }
            else if (invoice.TotalPrice <= invoice.ResidueForCalc && DateTime.Now <= invoice.EndDate)
            {
                invoice.IsPaid = 1;
            }
            else if (invoice.TotalPrice > invoice.ResidueForCalc)
            {
                //1=planlinib, 2 = gozlemede, 3=odenilib,4 odenilmeyib
                invoice.IsPaid = 2;
            }
            else
            {
                //1=planlinib, 2 = gozlemede, 3=odenilib,4 odenilmeyib
                invoice.IsPaid = 3;
            }
            //Deleting Income where equal == deleted incomeitem incomeId And incomeItems Count equal 1
            var income = _context.Incomes.Include(d => d.IncomeItems).FirstOrDefault(f => f.Id == incomeItem.IncomeId);
            if (income.IncomeItems.Count() == 1)
            {
                //first deleting incomeItems
                _context.IncomeItems.Remove(incomeItem);
                await _context.SaveChangesAsync();
                //than deleting income
                _context.Incomes.Remove(income);
                await _context.SaveChangesAsync();

                return incomeItem;
            }

            //deleting incomeItem without income
            _context.IncomeItems.Remove(incomeItem);
            await _context.SaveChangesAsync();

            return incomeItem;
        }
        #endregion

    }
}
