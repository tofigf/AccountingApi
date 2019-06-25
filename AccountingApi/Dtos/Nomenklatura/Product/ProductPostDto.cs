using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Nomenklatura.Product
{
    public class ProductPostDto
    {

        [MaxLength(75)]
        public string Name { get; set; }
        public string PhotoFile { get; set; }
        [MaxLength(75)]
        public string Category { get; set; }

        public bool IsServiceOrProduct { get; set; }

        public int? UnitId { get; set; }

        public double? Price { get; set; }

        public double? SalePrice { get; set; }
        [MaxLength(75)]
        public string Account { get; set; }
        [MaxLength(275)]
        public string Desc { get; set; }

        public int? Count { get; set; }

        public bool IsSale { get; set; }

        public bool IsPurchase { get; set; }

        public int? TaxId { get; set; }
    }
}
