using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class BalanceSheet
    {
        [Key]
        public int Id { get; set; }

        public double? DebitMoney { get; set; }
        public double? KreditMoney { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CompanyId { get; set; }
        public int? AccountsPlanId { get; set; }
        public int? InvoiceId { get; set; }
        public int? IncomeItemId { get; set; }
        public int? ExpenseInvoiceId { get; set; }
        public int? ExpenseItemId { get; set; }
        public int? ManualJournalId { get; set; }

        public virtual Company Company { get; set; }
        public virtual  AccountsPlan AccountsPlan { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual IncomeItem IncomeItem { get; set; }
        public virtual ManualJournal ManualJournal { get; set; }

    }
}
