using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Sale.Income
{
    public class IncomePutDto
    {
        public double? TotalPrice { get; set; }

     
        //public ICollection<IncomeItemGetEditDto> IncomeItemGetEditDtos { get; set; }
   

        //public IncomePutDto()
        //{
        //    IncomeItemGetEditDtos = new Collection<IncomeItemGetEditDto>();
        //}
    }
}
