using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Sale.Income
{
    public class IncomeGetDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string InvoiceNumber { get; set; }
        //contragent
        public string ContragentCompanyName { get; set; }
        public string ContragentFullname { get; set; }
        public double? TotalPrice { get; set; }
        public bool IsBank { get; set; }
        public double? PaidMoney { get; set; }
        public double? Residue { get; set; }
        public int InvoiceId { get; set; }
        public double? SumPaidMoney { get; set; }

        public double? TotalOneInvoice { get; set; }
    }
}
