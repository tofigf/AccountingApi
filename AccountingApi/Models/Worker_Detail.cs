using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class Worker_Detail
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(75)]
        public string FatherName { get; set; }
        [MaxLength(75)]
        public string Email { get; set; }
        [MaxLength(75)]
        public string Adress { get; set; }
        [MaxLength(75)]
        public string DSMF { get; set; }
        [MaxLength(75)]
        public string Voen { get; set; }
        [MaxLength(75)]
        public string Phone { get; set; }
        [MaxLength(75)]
        public string MobilePhone { get; set; }
        [MaxLength(75)]
        public string Education { get; set; }
        [MaxLength(75)]
        public string EducationLevel { get; set; }
        [MaxLength(20)]
        public string Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public int WorkerId { get; set; }

        public virtual Worker Worker { get; set; }
    }
}
