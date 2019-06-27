using AccountingApi.Dtos.Sale.Income;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ViewModel
{
    public class VwIncomePut
    {
        public IncomePutDto IncomePutDto { get; set; }
        public List<IncomeItemGetEditDto> IncomeItemGetEditDtos { get; set; }
    }
}
