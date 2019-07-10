using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ProcudureDto
{
    public class ProductExpenseAllDto
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public string Name { get; set; }
        public double? TotalPrice { get; set; }
    }
}
