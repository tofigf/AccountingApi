using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Sale.Income
{
    public class IncomeItemGetEditDto
    {
        //public string InvoiceNumber { get; set; }
        //public double? TotalOneInvoice { get; set; }
        public int Id { get; set; }
        public bool IsBank { get; set; }
        public double? PaidMoney { get; set; }
        public double? Residue { get; set; }
        public int InvoiceId { get; set; }
        //public int IncomeId { get; set; }
    }
}
