using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ProcudureDto
{
    public class WorkerFromQueryDto
    {
        public int? MonthNumber { get; set; }
        public int? EachMonthCount { get; set; }
        public string MonthName { get; set; }
    }
}
