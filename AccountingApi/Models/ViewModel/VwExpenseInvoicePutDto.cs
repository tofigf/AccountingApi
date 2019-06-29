using AccountingApi.Dtos.Company;
using AccountingApi.Dtos.Nomenklatura.Kontragent;
using AccountingApi.Dtos.Purchase.ExpenseInvoice;
using System.Collections.Generic;

namespace AccountingApi.Models.ViewModel
{
    public class VwExpenseInvoicePutDto
    {
        public ExpenseInvoicePutDto ExpenseInvoicePutDto { get; set; }
        public List<ExpenseInvoiceItemPutDto> ExpenseInvoiceItemPutDtos { get; set; }
        public CompanyPutProposalDto CompanyPutProposalDto { get; set; }
        public ContragentPutInProposalDto ContragentPutInProposalDto { get; set; }
    }
}
