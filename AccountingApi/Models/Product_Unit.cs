using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class Product_Unit
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(75)]
        public string Name { get; set; }

        public int? CompanyId { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual Company Company { get; set; }
    }
}
