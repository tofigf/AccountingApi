using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Sale.Income
{
    public class IncomeItemPostDto
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public double? TotalOneInvoice { get; set; }
        public bool IsBank { get; set; }
        public double? PaidMoney { get; set; }
        public double? Residue { get; set; }
    }
}
