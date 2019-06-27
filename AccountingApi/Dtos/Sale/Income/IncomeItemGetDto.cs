using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Sale.Income
{
    public class IncomeItemGetDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public double? TotalOneInvoice { get; set; }
        public double? Residue { get; set; }
        public bool IsBank { get; set; }
        public double? PaidMoney { get; set; }
        public int? InvoiceId { get; set; }
        public int? AccountDebitId { get; set; }
        public int? AccountKreditId { get; set; }
    }
}
