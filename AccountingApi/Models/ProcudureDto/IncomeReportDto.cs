using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ProcudureDto
{
    public class IncomeReportDto
    {
        public string InvoiceNumber { get; set; }
        //contragent
        public string CompanyName { get; set; }
        public double? Total { get; set; }

    }
}
