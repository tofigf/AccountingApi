using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class ExpenseInvoiceItem
    {
        [Key]
        public int Id { get; set; }
        public int? Qty { get; set; }
        public double? Price { get; set; }
        public double? TotalOneProduct { get; set; }

        public int? ProductId { get; set; }
        public int? ExpenseInvoiceId { get; set; }

        public virtual Product Product { get; set; }

        public virtual ExpenseInvoice ExpenseInvoice { get; set; }
    }
}
