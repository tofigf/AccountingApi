using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingApi.Models
{
    public class ExpenseItem
    {
        [Key]
        public int Id { get; set; }
        public double? Residue { get; set; }
        public double? TotalOneInvoice { get; set; }
        public double? PaidMoney { get; set; }
        public bool IsBank { get; set; } = false;
        public string InvoiceNumber { get; set; }
        public int ExpenseInvoiceId { get; set; }
        public int ExpenseId { get; set; }
        public int? AccountDebitId { get; set; }
        public int? AccountKreditId { get; set; }


        [ForeignKey("AccountDebitId")]
        public virtual AccountsPlan AccountsPlanDebit { get; set; }
        [ForeignKey("AccountKreditId")]
        public virtual AccountsPlan AccountsPlanKredit { get; set; }
        public virtual ICollection<BalanceSheet> BalanceSheets { get; set; }
        public virtual ExpenseInvoice ExpenseInvoice { get; set; }
        public virtual Expense Expense { get; set; }
    }
}
