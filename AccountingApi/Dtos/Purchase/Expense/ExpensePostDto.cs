using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Purchase.Expense
{
    public class ExpensePostDto
    {
        //paid money total
        public double? TotalPrice { get; set; }
    }
}
