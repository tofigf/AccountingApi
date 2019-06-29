using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Purchase.ExpenseInvoice
{
    public class ExpensiveInvoiceEditGetDto
    {
        public string ExpenseInvoiceNumber { get; set; }
        public DateTime? PreparingDate { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? EndDate { get; set; }
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

        public ICollection<ExpensiveInvoiceItemGetDto> ExpensiveInvoiceItemGetDtos { get; set; }

        public ExpensiveInvoiceEditGetDto()
        {
            ExpensiveInvoiceItemGetDtos = new Collection<ExpensiveInvoiceItemGetDto>();
        }
    }
}
