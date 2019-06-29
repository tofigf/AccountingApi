using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Purchase.ExpenseInvoice
{
    public class ExpenseInvoicePostDto
    {
        public string ExpenseInvoiceNumber { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalTax { get; set; }
        public double? Sum { get; set; }

        public string Desc { get; set; }

        public int? ContragentId { get; set; }

        public int CompanyId { get; set; }

        public int? TaxId { get; set; }

        public ICollection<ExpenseInvoiceItemPostDto> ExpenseInvoiceItemPostDtos { get; set; }

        public ExpenseInvoicePostDto()
        {
            ExpenseInvoiceItemPostDtos = new Collection<ExpenseInvoiceItemPostDto>();
        }
    }
}
