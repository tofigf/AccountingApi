using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Sale.Invoice
{
    public class InvoicePostDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalTax { get; set; }
        public double? Sum { get; set; }
        public string Desc { get; set; }
        [Required, Range(1, 4)]
        public byte IsPaid { get; set; } = 1;
        public int? ContragentId { get; set; }
        public int CompanyId { get; set; }
        public int? TaxId { get; set; }
        public int? AccountDebitId { get; set; }
        public int? AccountKreditId { get; set; }


        public ICollection<InvoiceItemPostDto> InvoiceItemPostDtos { get; set; }

        public InvoicePostDto()
        {
            InvoiceItemPostDtos = new Collection<InvoiceItemPostDto>();
        }
    }
}
