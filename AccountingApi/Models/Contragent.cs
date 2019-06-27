using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class Contragent
    {
        [Key]
        public int Id { get; set; }

        public string PhotoFile { get; set; }
        [MaxLength(150)]
        public string PhotoUrl { get; set; }
        [MaxLength(100)]
        public string CompanyName { get; set; }
        [MaxLength(100)]
        public string Fullname { get; set; }
        [MaxLength(75)]
        public string Position { get; set; }
        [MaxLength(75)]
        public string FieldOfActivity { get; set; }
        [MaxLength(75)]
        public string Phone { get; set; }
        [MaxLength(75)]
        public string Email { get; set; }

        [MaxLength(75)]
        public string VOEN { get; set; }

        public DateTime CreetedAt { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsCostumer { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public virtual ICollection<Contragent_Detail> Contragent_Details { get; set; }
        public virtual ICollection<Proposal> Proposals { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Income> Incomes { get; set; }
    }
}
