using System;

namespace AccountingApi.Dtos.Purchase.Expense
{
    public class ExpenseExInvoiceGetDto
    {
        public int ExpenseInvoiceId { get; set; }
        public string ExpenseInvoiceNumber { get; set; }
        public double? TotalPrice { get; set; }
        public double? Residue { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
