using AccountingApi.Dtos.Company;
using AccountingApi.Dtos.Nomenklatura.Kontragent;
using AccountingApi.Dtos.Sale.Invoice;
using EOfficeAPI.Dtos.Sale.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ViewModel
{
    public class VwInvoicePut
    {
        public InvoicePutDto InvoicePutDto { get; set; }
        public List<InvoiceItemPutDto> InvoiceItemPutDtos { get; set; }
        public CompanyPutProposalDto CompanyPutProposalDto { get; set; }
        public ContragentPutInProposalDto ContragentPutInProposalDto { get; set; }
    }
}
