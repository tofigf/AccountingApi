using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class AccountsPlan
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string AccPlanNumber { get; set; }
        [MaxLength(350)]
        public string Name { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> Level { get; set; }
        public string Obeysto { get; set; }
        public Nullable<bool> ContraAccount { get; set; }
        public Nullable<double> Debit { get; set; }
        public Nullable<double> Kredit { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

 
    }
}
