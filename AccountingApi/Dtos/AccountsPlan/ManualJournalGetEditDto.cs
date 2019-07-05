using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.AccountsPlan
{
    public class ManualJournalGetEditDto
    {
        public string JurnalNumber { get; set; }
        public string JurnalName { get; set; }
        public DateTime? Date { get; set; }
        public string Desc { get; set; }
        public double? Price { get; set; }

        //Manual Journal
        public string AccountsPlanDebitAccPlanNumber { get; set; }
        public string AccountsPlanDebitName { get; set; }
        public string AccountsPlanKreditAccPlanNumber { get; set; }
        public string AccountsPlanKreditName { get; set; }



        public int? ContragentId { get; set; }
        public int? AccountDebitId { get; set; }
        public int? AccountKreditId { get; set; }
        public int? OperationCategoryId { get; set; }
    }
}
