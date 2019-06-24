using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Nomenklatura.Worker
{
    public class WorkerPutDto
    {
        [MaxLength(75)]
        public string Name { get; set; }
        [MaxLength(75)]
        public string SurName { get; set; }
        [MaxLength(75)]
        public string Positon { get; set; }
        [Required]
        public double Salary { get; set; }
        [MaxLength(75)]
        public string Departament { get; set; }
        [MaxLength(75)]
        public string PartofDepartament { get; set; }
        [MaxLength(75)]
        public string Role { get; set; }

        public string PhotoFile { get; set; }

        public bool IsState { get; set; }

        public string Voen { get; set; }
        [MaxLength(75)]
        public string FatherName { get; set; }
        [MaxLength(75)]
        public string Email { get; set; }
        [MaxLength(75)]
        public string Adress { get; set; }
        [MaxLength(75)]
        public string DSMF { get; set; }
        [MaxLength(75)]
        public string Phone { get; set; }
        [MaxLength(75)]
        public string MobilePhone { get; set; }
        [MaxLength(75)]
        public string Education { get; set; }
        [MaxLength(75)]
        public string EducationLevel { get; set; }

        public string Gender { get; set; }

        public DateTime? Birthday { get; set; }    }
}
