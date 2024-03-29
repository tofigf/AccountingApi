﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Sale.Income
{
    public class IncomeInvoiceGetDto
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public double? TotalPrice { get; set; }
        public double? Residue { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AccountDebitId { get; set; }
        public int? AccountKreditId { get; set; }

    }
}
