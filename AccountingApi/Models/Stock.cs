using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }

        public double? Price { get; set; }

        public double? SalePrice { get; set; }

        [MaxLength(275)]
        public string Desc { get; set; }

        public int? Count { get; set; }

        public bool IsSale { get; set; } = false;

        public bool IsPurchase { get; set; } = false;
        public bool IsDeleted { get; set; } = false;


        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
