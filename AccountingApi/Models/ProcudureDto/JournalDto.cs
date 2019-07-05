using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ProcudureDto
{
    public class JournalDto
    {
        public string Number { get; set; }
        public string AccDebitNumber { get; set; }
        public string DebitName { get; set; }
        public string AccKreditNumber { get; set; }
        public string KreditName { get; set; }
        public double? Price { get; set; }
        public DateTime? Date { get; set; }
        public string CategoryName { get; set; }
    }
}
