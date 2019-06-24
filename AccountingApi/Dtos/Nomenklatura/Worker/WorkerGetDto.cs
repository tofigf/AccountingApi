using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Nomenklatura.Worker
{
    public class WorkerGetDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
       
        public string SurName { get; set; }
      
        public double Salary { get; set; }
       
        public string Departament { get; set; }
     
        public string PhotoUrl { get; set; }

        public bool IsState { get; set; }

        //public ICollection<Worker_DetailGetDto> Worker_DetailGetDtos { get; set; }


        //public WorkerGetDto()
        //{
        //    Worker_DetailGetDtos = new Collection<Worker_DetailGetDto>();
        //}
    }
}
