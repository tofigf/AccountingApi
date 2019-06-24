using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Sale.Invoice
{
    public class InvoiceGetDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }

        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? TotalPrice { get; set; }

        //contragent companyname
        public string ContragentCompanyName { get; set; }

        public byte IsPaid { get; set; }
    }
}
