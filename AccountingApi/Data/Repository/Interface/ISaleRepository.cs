using AccountingApi.Dtos.Sale.Proposal;
using AccountingApi.Models;
using EOfficeAPI.Helpers.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository.Interface
{
  public  interface ISaleRepository
    {
        //Proposal
        #region Proposal
        //Post
        //Creating proposal repo
        Task<Proposal> CreateProposal(Proposal proposalRepo, int? companyId);
        //creating proposalitem repo
        Task<IEnumerable<ProposalItem>> CreateProposalItems(IEnumerable<ProposalItem> items, int? proposalId);
        //Email
        ProposalSentMail CreateProposalSentMail(int? proposalId, string email);

        Proposal GetProposalByToken(string token);
        //Get 
        //get company by id
        Task<Company> GetEditCompany(int? companyId);
        //get contragent by id
        Task<Contragent> GetEditContragent(int? companyId);
        // get all proposal
        Task<List<ProposalGetDto>> GetProposal(PaginationParam productParam, int? companyId);
        //get proposal detail
        Task<Proposal> GetDetailProposal(int? proposalId, int? companyId);
        //get proposal by id
        Task<Proposal> GetEditProposal(int? proposalId, int? companyId);
        //get proposalitem by proposalid
        Task<List<ProposalItem>> GetEditProposalItem(int? proposalId);
        //get contragent by contragent id
        Task<Contragent> GetContragentProposal(int? companyId, int? proposalId);
        //Put
        //when creating proposal edit company
        Task<Company> EditCompany(Company company, int? companyId, int? userId);
        //when creating proposal edit company
        Task<Contragent> EditContragent(Contragent contragent, int? companyId);
        // edit proposal by id
        Task<Proposal> EditProposal(Proposal proposal, List<ProposalItem> proposalItems, int? proposalId);
        //Checking
        Task<bool> CheckProposal(int? currentUserId, int? companyId);
        Task<bool> CheckProposalProductId(IEnumerable<ProposalItem> proposalItem);
        Task<bool> CheckProposalId(int? productId, int? companyId);
        Task<bool> CheckContragentId(int? contragentId, int? companyId);
        //Delete
        Task<ProposalItem> DeleteProposalItem(int? proposalItemId);
        Task<Proposal> DeleteProposal(int? companyId, int? proposalId);

        #endregion
    }
}
