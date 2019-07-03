using AccountingApi.Dtos.Purchase.Expense;
using AccountingApi.Dtos.Sale.Income;
using System.Collections.Generic;

namespace AccountingApi.Models.ViewModel
{
    public class VwExpensePut
    {
        public IncomePutDto IncomePutDto { get; set; }
        public List<ExpenseItemGetDto> ExpenseItemGetDtos { get; set; }
    }
}
