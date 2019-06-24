using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class Worker
    {

        [Key]
        public int Id { get; set; }
        [MaxLength(75)]
        public string Name { get; set; }
        [MaxLength(75)]
        public string SurName { get; set; }
        [MaxLength(75)]
        public string Positon { get; set; }

        public double? Salary { get; set; }
        [MaxLength(75)]
        public string Departament { get; set; }
        [MaxLength(75)]
        public string PartofDepartament { get; set; }
        [MaxLength(75)]
        public string Role { get; set; }

        public DateTime RegisterDate { get; set; }

        public string PhotoFile { get; set; }

        public string PhotoUrl { get; set; }

        public bool IsState { get; set; }

        public bool IsDeleted { get; set; } = false;


        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<Worker_Detail> Worker_Details { get; set; }
    }
}
