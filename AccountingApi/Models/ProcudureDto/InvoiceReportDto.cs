using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ProcudureDto
{
    public class InvoiceReportDto
    {
        public double? Total { get; set; }
        public string Status { get; set; }
        public byte? IsPaid { get; set; }
    }
}
