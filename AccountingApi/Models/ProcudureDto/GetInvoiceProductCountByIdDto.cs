using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ProcudureDto
{
    public class GetInvoiceProductCountByIdDto
    {
        public int Qty { get; set; }
        public string CompanyName { get; set; }
        public double? Price { get; set; }
    }
}
