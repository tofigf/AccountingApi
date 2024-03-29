﻿using System;

namespace AccountingApi.Dtos.Purchase.Expense
{
    public class ExpenseItemPostDto
    {
        public int ExpenseInvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public double? TotalOneInvoice { get; set; }
        public bool IsBank { get; set; }
        public double? PaidMoney { get; set; }
        public double? Residue { get; set; }
        public int? AccountDebitId { get; set; }
        public int? AccountKreditId { get; set; }
        public DateTime? Date { get; set; }
    }
}
