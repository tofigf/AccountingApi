using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Sale.Income
{
    public class IncomeInvoiceEditGetDto
    {
        public double? ResidueForCalc { get; set; }
        public int Id { get; set; }

        public string ContragentCompanyName { get; set; }

        public ICollection<IncomeItemInvoiceGetDto> IncomeItemInvoiceGetDtos { get; set; }

        public IncomeInvoiceEditGetDto()
        {
            IncomeItemInvoiceGetDtos = new Collection<IncomeItemInvoiceGetDto>();
        }
    }
}
