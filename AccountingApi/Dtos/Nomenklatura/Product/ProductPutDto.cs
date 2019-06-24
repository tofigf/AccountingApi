using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Nomenklatura.Product
{
    public class ProductPutDto
    {
    

        [MaxLength(75)]
        public string Name { get; set; }
        public string PhotoFile { get; set; }
        [MaxLength(75)]
        public string Category { get; set; }

        public bool IsServiceOrProduct { get; set; }

        public int UnitId { get; set; }

        public double? Price { get; set; }

        public double? SalePrice { get; set; }

        public string Account { get; set; }

        public string Desc { get; set; }

        public int? Count { get; set; }

        public bool IsSale { get; set; }

        public bool IsPurchase { get; set; }

        public int? TaxId { get; set; }
    }
}
