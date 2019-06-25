using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.AccountsPlan
{
    public class AccountsPlanGetDto
    {
        public int Id { get; set; }
        public string AccPlanNumber { get; set; }
        public string Name { get; set; }
        public Nullable<int> Level { get; set; }
        public string Category { get; set; }

    }
}
