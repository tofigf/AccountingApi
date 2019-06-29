using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        //paid money total
        public double? TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        public int CompanyId { get; set; }
        public int ContragentId { get; set; }

        public virtual Contragent Contragent { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<ExpenseItem> ExpenseItems { get; set; }
    }
}
