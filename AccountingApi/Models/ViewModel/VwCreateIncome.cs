using AccountingApi.Dtos.Sale.Income;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ViewModel
{
    public class VwCreateIncome
    {
        public List<IncomeItemPostDto> IncomeItemPostDtos { get; set; }
        public IncomePostDto IncomePostDto { get; set; }
        public int[] Ids { get; set; }
    }
}
