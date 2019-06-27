using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class IncomeItem
    {
        [Key]
        public int Id { get; set; }
        public double? Residue { get; set; }
        public double? TotalOneInvoice { get; set; }
        public double? PaidMoney { get; set; }
        public bool IsBank { get; set; } = false;
        [MaxLength(300)]
        public string InvoiceNumber { get; set; }
        public int InvoiceId { get; set; }
        public int IncomeId { get; set; }
        public DateTime? Date { get; set; }
        public int? AccountDebitId { get; set; }
        public int? AccountKreditId { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Income Income { get; set; }

        [ForeignKey("AccountDebitId")]
        public virtual AccountsPlan AccountsPlanDebit { get; set; }
        [ForeignKey("AccountKreditId")]
        public virtual AccountsPlan AccountsPlanKredit { get; set; }

        public virtual ICollection<BalanceSheet> BalanceSheets { get; set; }

    }
}
