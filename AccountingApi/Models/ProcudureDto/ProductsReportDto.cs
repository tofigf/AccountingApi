using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ProcudureDto
{
    public class ProductsReportDto
    {
        public int SumQty { get; set; }
        public string Name { get; set; }
        public int PercentOfTotal { get; set; }

        public string Color { get; set; }

    }
}
