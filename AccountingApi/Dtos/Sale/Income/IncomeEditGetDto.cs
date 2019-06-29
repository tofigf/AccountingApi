using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Sale.Income
{
    public class IncomeEditGetDto
    {

        public int Id { get; set; }
        //contragent
        public string ContragentCompanyName { get; set; }
        public string ContragentFullname { get; set; }
        public double? TotalPrice { get; set; }

        public ICollection<IncomeItemGetDto> IncomeItemGetDtos { get; set; }

        public IncomeEditGetDto()
        {
            IncomeItemGetDtos = new Collection<IncomeItemGetDto>();
        }
    }
}


