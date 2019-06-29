using AccountingApi.Dtos.Company;
using AccountingApi.Dtos.Nomenklatura.Kontragent;
using AccountingApi.Dtos.Purchase.ExpenseInvoice;
using System.Collections.Generic;

namespace AccountingApi.Models.ViewModel
{
    public class VwCreateExpenseInvoice
    {
        public ExpenseInvoicePostDto ExpenseInvoicePostDto { get; set; }

        public List<ExpenseInvoiceItemPostDto> ExpenseInvoiceItemPostDtos { get; set; }
        public CompanyPutProposalDto CompanyPutProposalDto { get; set; }

        public ContragentPutInProposalDto ContragentPutInProposalDto { get; set; }
    }
}
