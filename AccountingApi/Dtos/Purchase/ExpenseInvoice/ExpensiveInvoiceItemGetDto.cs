using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Purchase.ExpenseInvoice
{
    public class ExpensiveInvoiceItemGetDto
    {
        public int Id { get; set; }
        public int? Qty { get; set; }
        public double? Price { get; set; }
        public double? TotalOneProduct { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
