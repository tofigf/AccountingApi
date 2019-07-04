using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Account
{
    public class BalanceSheetReturnDto
    {
        public string AccPlanNumber { get; set; }
        public string Name { get; set; }
        public double? startCircleDebit { get; set; }
        public double? startCircleKredit { get; set; }
        public double? allCircleDebit { get; set; }
        public double? allCircleKredit { get; set; }
        public double? endCircleDebit { get; set; }
        public double? endCircleKredit { get; set; }
    }
}
