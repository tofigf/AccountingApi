using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ProcudureDto
{
    public class InvoiceFromQueryDto
    {
        public double? Total { get; set; }
        public byte? IsPaid { get; set; }
    }
}
