using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.AccountsPlan
{
    public class ManualJournalPostDto
    {
        [MaxLength(300)]
        public string JurnalNumber { get; set; }
        [MaxLength(300)]
        public string JurnalName { get; set; }
        public DateTime? Date { get; set; }
        [MaxLength(300)]
        public string Desc { get; set; }
        public double? Price { get; set; }

        public int? ContragentId { get; set; }
        public int CompanyId { get; set; }
        public int? AccountDebitId { get; set; }
        public int? AccountKreditId { get; set; }
        public int? OperationCategoryId { get; set; }
    }
}
