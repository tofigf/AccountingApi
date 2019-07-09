using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ProcudureDto
{
    public class InvoiceReportByContragentDto
    {
        public  string CompanyName { get; set; }
        public int? Qty { get; set; }
        public double? Price { get; set; }
    }
}
