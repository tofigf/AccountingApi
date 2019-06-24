using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Company
{
    public class CompanyGetDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string PhotoUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Postion { get; set; }
        public string FieldOfActivity { get; set; }
        public string VOEN { get; set; }
        public string Culture { get; set; }
        public string Weekday { get; set; }


    }
}
