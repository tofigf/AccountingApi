using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Purchase.ExpenseInvoice
{
    public class ExpenseInvoiceItemPostDto
    {
        public int? Qty { get; set; }
        public double? Price { get; set; }
        public double? TotalOneProduct { get; set; }

        public int? ProductId { get; set; }
        public int? ExpenseInvoiceId { get; set; }
    }
}
