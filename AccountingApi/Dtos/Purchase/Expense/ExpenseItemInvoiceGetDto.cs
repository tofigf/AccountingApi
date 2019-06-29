﻿using System;

namespace AccountingApi.Dtos.Purchase.Expense
{
    public class ExpenseItemInvoiceGetDto
    {
        public int Id { get; set; }
        public bool IsBank { get; set; }
        public double? PaidMoney { get; set; }
        public DateTime ExpenseCreatedAt { get; set; }
        public double? TotalOneInvoice { get; set; }
    }
}