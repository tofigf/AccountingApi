using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Purchase.Expense
{
    public class ExpenseExInvoiceEditGetDto
    {
        public double? ResidueForCalc { get; set; }
        public int Id { get; set; }

        public string ContragentCompanyName { get; set; }

        public ICollection<ExpenseItemInvoiceGetDto> ExpenseItemInvoiceGetDtos { get; set; }

        public ExpenseExInvoiceEditGetDto()
        {
            ExpenseItemInvoiceGetDtos = new Collection<ExpenseItemInvoiceGetDto>();
        }
    }
}
