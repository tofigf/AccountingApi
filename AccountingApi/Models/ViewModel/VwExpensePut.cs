using AccountingApi.Dtos.Purchase.Expense;
using System.Collections.Generic;

namespace AccountingApi.Models.ViewModel
{
    public class VwExpensePut
    {
        //public IncomePutDto IncomePutDto { get; set; }
        public List<ExpenseItemGetDto> ExpenseItemGetDtos { get; set; }
    }
}
