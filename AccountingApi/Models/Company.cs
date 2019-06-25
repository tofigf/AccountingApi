using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        public string PhotoFile { get; set; }
        [MaxLength(150)]
        public string PhotoUrl { get; set; }
        [MaxLength(75)]
        public string CompanyName { get; set; }
        [MaxLength(75)]
        public string Name { get; set; }
        [MaxLength(75)]
        public string Surname { get; set; }
        [MaxLength(75)]
        public string Postion { get; set; }
        [MaxLength(75)]
        public string FieldOfActivity { get; set; }
        [MaxLength(100)]
        public string VOEN { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }
        [MaxLength(75)]
        public string Street { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(20)]
        public string Mobile { get; set; }
        [MaxLength(30)]
        public string Website { get; set; }
        [MaxLength(30)]
        public string Linkedin { get; set; }
        [MaxLength(30)]
        public string Facebok { get; set; }
        [MaxLength(30)]
        public string Instagram { get; set; }
        [MaxLength(30)]
        public string Behance { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }

        public bool IsCompany { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Tax> Taxes { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Product_Unit> Product_Units { get; set; }
        public virtual ICollection<Contragent> Contragents { get; set; }
        public virtual ICollection<AccountsPlan> AccountsPlans { get; set; }
        public virtual ICollection<Proposal> Proposals { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<BalanceSheet> BalanceSheets { get; set; }



    }
}
