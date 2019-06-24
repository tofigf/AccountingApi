using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Nomenklatura.Product
{
    public class ProductGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string Category { get; set; }
        public double? SalePrice { get; set; }
        public double? Price { get; set; }
        public bool IsSale { get; set; }
        public bool IsPurchase { get; set; }
        public string UnitName { get; set; }

        //public ProductGetDto()
        //{
        //    StockGet = new Collection<StockGetDto>();
        //}
    }
}
