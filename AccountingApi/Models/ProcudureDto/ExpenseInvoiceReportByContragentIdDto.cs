using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ProcudureDto
{
    public class ExpenseInvoiceReportByContragentIdDto
    {
        public string ExpenseInvoiceNumber { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
