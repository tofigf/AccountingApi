using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Sale.Income
{
    public class IncomeInvoiceGetDto
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public double? TotalPrice { get; set; }
        public double? Residue { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
