using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Nomenklatura.Kontragent
{
    public class ContragentGetDto
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public string CompanyName { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsCostumer { get; set; }
        public string VOEN { get; set; }


    }
}
