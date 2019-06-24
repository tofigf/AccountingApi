using AccountingApi.Data.Repository.Interface;
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

    }
}
