using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class Proposal
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(500)]
        public string ProposalNumber { get; set; }

        public DateTime? PreparingDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public double? TotalPrice { get; set; }
        public double? TotalTax { get; set; }
        public double? Sum { get; set; }

        [MaxLength(300)]
        public string Desc { get; set; }
        public bool IsDeleted { get; set; } = false;

        public int? ContragentId { get; set; }

        public int CompanyId { get; set; }
        public int? TaxId { get; set; }

        public virtual Company Company { get; set; }
        public virtual Contragent Contragent { get; set; }
        public virtual Tax Tax { get; set; }

        public virtual ICollection<ProposalItem> ProposalItems { get; set; }

        public virtual ICollection<ProposalSentMail> ProposalSentMails { get; set; }
    }
}
