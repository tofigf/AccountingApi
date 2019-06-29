using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Sale.Income
{
    public class IncomeItemInvoiceGetDto
    {
        public int Id { get; set; }
        public bool IsBank { get; set; }
        public double? PaidMoney { get; set; }
        public DateTime IncomeCreatedAt { get; set; }
        public double? TotalOneInvoice { get; set; }
        public int? AccountDebitId { get; set; }
        public int? AccountKreditId { get; set; }
        public DateTime? Date  { get; set; }

    }
}
