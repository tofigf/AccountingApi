using AccountingApi.Dtos.Purchase.Expense;
using System.Collections.Generic;

namespace AccountingApi.Models.ViewModel
{
    public class VwCreateExpense
    {
        public List<ExpenseItemPostDto> ExpenseItemPostDtos { get; set; }
        public ExpensePostDto ExpensePostDto { get; set; }
    }
}
