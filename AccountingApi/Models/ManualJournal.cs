using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class ManualJournal
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(300)]
        public string JurnalNumber { get; set; }
        [MaxLength(300)]
        public string JurnalName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Date { get; set; }
        [MaxLength(300)]
        public string Desc { get; set; }
        public double? Price { get; set; }

        public int? ContragentId { get; set; }
        public int CompanyId { get; set; }
        public int? AccountDebitId { get; set; }
        public int? AccountKreditId { get; set; }
        public int? OperationCategoryId { get; set; }

        public virtual Company Company { get; set; }
        public virtual Contragent Contragent { get; set; }
        public virtual OperationCategory OperationCategory { get; set; }

        [ForeignKey("AccountDebitId")]
        public virtual AccountsPlan AccountsPlanDebit { get; set; }
        [ForeignKey("AccountKreditId")]
        public virtual AccountsPlan AccountsPlanKredit { get; set; }
        public virtual ICollection<BalanceSheet> BalanceSheets { get; set; }


    }
}
