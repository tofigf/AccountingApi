using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class AccountsPlan
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string AccPlanNumber { get; set; }
        [MaxLength(350)]
        public string Name { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Level { get; set; }
        [MaxLength(350)]
        public string Obeysto { get; set; }
        [MaxLength(350)]
        public string Category { get; set; }
        public Nullable<bool> ContraAccount { get; set; }
        public Nullable<double> Debit { get; set; }
        public Nullable<double> Kredit { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [InverseProperty("AccountsPlanDebit")]
        public virtual ICollection<Invoice> InvoicesDebit { get; set; }
        [InverseProperty("AccountsPlanKredit")]
        public virtual ICollection<Invoice> InvoicesKredit { get; set; }

        [InverseProperty("AccountsPlanDebit")]
        public virtual ICollection<IncomeItem> IncomeItemsDebit { get; set; }
        [InverseProperty("AccountsPlanKredit")]
        public virtual ICollection<IncomeItem> IncomeItemsKredit { get; set; }
        public virtual ICollection<BalanceSheet> BalanceSheets { get; set; }


    }
}
