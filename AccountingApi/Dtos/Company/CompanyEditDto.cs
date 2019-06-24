using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Company
{
    public class CompanyEditDto
    {
       
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string PhotoUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Postion { get; set; }
        public string FieldOfActivity { get; set; }
        public string VOEN { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Website { get; set; }
        public string Linkedin { get; set; }
        public string Facebok { get; set; }
        public string Instagram { get; set; }
        public string Behance { get; set; }
        public string City { get; set; }
        public string Email { get; set; }



        public bool IsCompany { get; set; }
        public int UserId { get; set; }

    }
}
