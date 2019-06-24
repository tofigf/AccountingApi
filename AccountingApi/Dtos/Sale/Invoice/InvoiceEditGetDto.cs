using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Sale.Invoice
{
    public class InvoiceEditGetDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalTax { get; set; }
        public double? Sum { get; set; }
        public int? ContragentId { get; set; }
        public int? TaxId { get; set; }
        public double? TaxRate { get; set; }
        //company
        public string CompanyCompanyName { get; set; }
        public string CompanyVOEN { get; set; }
        //contragent
        public string ContragentCompanyName { get; set; }
        public string ContragentVoen { get; set; }

        public ICollection<InvoiceItemGetDto> InvoiceItemGetDtos { get; set; }

        public InvoiceEditGetDto()
        {
            InvoiceItemGetDtos = new Collection<InvoiceItemGetDto>();
        }
    }
}
