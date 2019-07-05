using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class OperationCategory
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string ShortName { get; set; }

        public virtual ICollection<ManualJournal> ManualJournals { get; set; }
    }
}
