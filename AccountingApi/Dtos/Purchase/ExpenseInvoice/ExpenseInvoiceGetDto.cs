using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Purchase.ExpenseInvoice
{
    public class ExpenseInvoiceGetDto
    {
        public int Id { get; set; }
        public string ExpenseInvoiceNumber { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? TotalPrice { get; set; }
        public byte IsPaid { get; set; }
        //contragent companyname
        public string ContragentCompanyName { get; set; }

    }
}
