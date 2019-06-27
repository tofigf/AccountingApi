using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250)]
        public string InvoiceNumber { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public double? TotalPrice { get; set; }
        //for calculating totalprice  every adding income
        public double? ResidueForCalc { get; set; }
        public double? TotalTax { get; set; }
        public double? Sum { get; set; }
        [MaxLength(300)]
        public string Desc { get; set; }
        [Required, Range(1, 4)]
        public byte IsPaid { get; set; } = 1;
        public bool IsDeleted { get; set; } = false;

        public int? ContragentId { get; set; }
        public int CompanyId { get; set; }
        public int? TaxId { get; set; }
        public int? AccountDebitId { get; set; }
        public int? AccountKreditId { get; set; }

        public virtual Company Company { get; set; }
        public virtual Contragent Contragent { get; set; }
        public virtual Tax Tax { get; set; }

        [ForeignKey("AccountDebitId")]
        public virtual AccountsPlan AccountsPlanDebit { get; set; }
        [ForeignKey("AccountKreditId")]
        public virtual AccountsPlan AccountsPlanKredit { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual ICollection<InvoiceSentMail> InvoiceSentMails { get; set; }
        public virtual ICollection<BalanceSheet> BalanceSheets { get; set; }
        public virtual ICollection<IncomeItem> IncomeItems { get; set; }
    }
}
