using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ProcudureDto
{
    public class ContragentFromQueryDto
    {
        public int? MonthNumber { get; set; }
        public string MonthName { get; set; }
        public int? SellerCount { get; set; }
        //public string SellerName { get; set; }
        public int? CostumerCount { get; set; }
        //public string CostumerName { get; set; }
    }
}
