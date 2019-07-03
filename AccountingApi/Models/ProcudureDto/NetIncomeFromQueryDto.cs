using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ProcudureDto
{
    public class NetIncomeFromQueryDto
    {
        public int? MonthNumber { get; set; }
        public double? InTotal { get; set; }
        public double? ExTotal { get; set; }
        public double? NetTotal { get; set; }
    }
}
