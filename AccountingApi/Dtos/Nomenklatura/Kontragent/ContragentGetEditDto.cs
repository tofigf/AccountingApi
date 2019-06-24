using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Nomenklatura.Kontragent
{
    public class ContragentGetEditDto
    {
        public string PhotoUrl { get; set; }
        public string CompanyName { get; set; }
        public string Fullname { get; set; }
        public string Position { get; set; }
        public string FieldOfActivity { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string VOEN { get; set; }
        public bool IsCostumer { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Adress { get; set; }
        public string WebSite { get; set; }
        public string Linkedin { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Behance { get; set; }
    }
}
