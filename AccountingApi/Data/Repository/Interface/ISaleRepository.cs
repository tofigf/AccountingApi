using AccountingApi.Dtos.Sale.Invoice;
using AccountingApi.Dtos.Sale.Proposal;
using AccountingApi.Models;
using EOfficeAPI.Dtos.Sale.Invoice;
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

        //Invoice
        #region Invoice
        //Post
        Task<Invoice> CreateInvoice(Invoice invoice, int? companyId);
        Task<List<InvoiceItem>> CreateInvoiceItems(List<InvoiceItem> items, int? invoiceId);
        //Get
        //get all invoice
        Task<List<InvoiceGetDto>> GetInvoice(PaginationParam productParam, int? companyId);
        //get for edit invoice by id
        Task<Invoice> GetDetailInvoice(int? invoiceId, int? companyId);

        Task<Invoice> GetEditInvoice(int? invoiceId, int? companyId);
        //get for edit invoice by id
        Task<List<InvoiceItem>> GetEditInvoiceItem(int? invoiceId);
        Task<Contragent> GetContragentInvoice(int? companyId, int? invoiceId);
        //Put
        // edit edit by id
        Task<Invoice> EditInvoice(Invoice invoice, List<InvoiceItem> invoiceItems, int? invoiceId);
        //Delete
        Task<InvoiceItem> DeleteInvoiceItem(int? invoiceItemId);
        Task<Invoice> DeleteInvoice(int? companyId, int? invoiceId);
        //Checking
        Task<bool> CheckInvoice(int? currentUserId, int? companyId);
        Task<bool> CheckInvoiceProductId(List<InvoiceItem> invoiceItems);
        Task<bool> CheckInvoiceId(int? invoiceId, int? companyId);
        Task<bool> CheckInvoiceItem(int? invoiceId, List<InvoiceItem> invoiceItems);
        //checking exist income
        //Task<bool> CheckExistIncomeByInvoiceId(int? invoiceId);
        //Email
        InvoiceSentMail CreateInvoiceSentMail(int? invoiceId, string email);
        Invoice GetInvoiceByToken(string token);
        //Accounting Update
        InvoicePutDto UpdateAccountDebit(int? invoiceId, int? companyId, InvoicePutDto invoice, int? OldDebitId);
       InvoicePutDto UpdateAccountKredit(int? invoiceId, int? companyId, InvoicePutDto invoice, int? OldKeditId);

            #endregion

        }
}
