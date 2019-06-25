using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Sale.Invoice
{
    public class InvoicePutDto
    {
        public string InvoiceNumber { get; set; }

        public DateTime? PreparingDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double? TotalPrice { get; set; }
        public double? TotalTax { get; set; }
        public double? Sum { get; set; }

        public string Desc { get; set; }

        public int? TaxId { get; set; }

        public int? ContragentId { get; set; }
        public int? AccountDebitId { get; set; }
        public int? AccountKreditId { get; set; }


        //public ICollection<InvoiceItemPostDto> InvoiceItemPostDtos { get; set; }

        //public InvoicePutDto()
        //{
        //    InvoiceItemPostDtos = new Collection<InvoiceItemPostDto>();
        //}
    }
}
