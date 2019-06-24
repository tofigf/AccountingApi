using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Nomenklatura.Worker
{
    public class WorkerEditDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public string Positon { get; set; }

        public double Salary { get; set; }

        public bool IsState { get; set; }

        public string Voen { get; set; }

        public string Departament { get; set; }

        public string PartofDepartament { get; set; }

        public string Role { get; set; }

        public string PhotoUrl { get; set; }

         public string FatherName { get; set; }

        public string Email { get; set; }

        public string Adress { get; set; }

        public string DSMF { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Education { get; set; }

        public string EducationLevel { get; set; }

        public string Gender { get; set; }

        public DateTime? Birthday { get; set; }
      

        //public ICollection<Worker_DetailEditDto> Worker_DetailEditDtos { get; set; }


        //public WorkerEditDto()
        //{
        //    Worker_DetailEditDtos = new Collection<Worker_DetailEditDto>();
        //}

    }
}
