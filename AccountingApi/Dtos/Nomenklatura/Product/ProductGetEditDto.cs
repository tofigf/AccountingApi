using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Nomenklatura.Product
{
    public class ProductGetEditDto
    {
        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        public string Category { get; set; }

        public bool IsServiceOrProduct { get; set; }

        public int UnitId { get; set; }
        public string UnitName { get; set; }

        public double? Price { get; set; }

        public double? SalePrice { get; set; }

        public string Desc { get; set; }

        public bool IsSale { get; set; }

        public bool IsPurchase { get; set; }

    }
}
