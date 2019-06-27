using AccountingApi.Dtos.Company;
using AccountingApi.Dtos.Nomenklatura.Kontragent;
using AccountingApi.Dtos.Sale.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ViewModel
{
    public class VwInvoice
    {
        public InvoicePostDto InvoicePostDto { get; set; }

        public List<InvoiceItemPostDto> InvoiceItemPosts { get; set; }
        public CompanyPutProposalDto CompanyPutProposalDto { get; set; }

        public ContragentPutInProposalDto ContragentPutInProposalDto { get; set; }
    }
}
