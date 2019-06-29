using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(75)]
        public string Name { get; set; }
        public string PhotoFile { get; set; }
        [MaxLength(150)]
        public string PhotoUrl { get; set; }
        [MaxLength(75)]
        public string Category { get; set; }

        public bool IsServiceOrProduct { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int? UnitId { get; set; }

        public int CompanyId { get; set; }

        public virtual Product_Unit Unit { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<ExpenseInvoiceItem> ExpenseInvoices { get; set; }
    }
}
