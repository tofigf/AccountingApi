using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Nomenklatura.Kontragent
{
    public class ContragentPostDto
    {
        public string PhotoFile { get; set; }
        [MaxLength(100)]
        public string CompanyName { get; set; }
        [MaxLength(100)]
        public string Fullname { get; set; }
        [MaxLength(75)]
        public string Position { get; set; }
        [MaxLength(75)]
        public string FieldOfActivity { get; set; }
        [MaxLength(75)]
        public string Phone { get; set; }
        [MaxLength(75)]
        public string Email { get; set; }

        public string VOEN { get; set; }

        public bool IsCostumer { get; set; }

        [MaxLength(75)]
        public string City { get; set; }
        [MaxLength(75)]
        public string Country { get; set; }
        [MaxLength(75)]
        public string Adress { get; set; }
        [MaxLength(75)]
        public string WebSite { get; set; }
        [MaxLength(75)]
        public string Linkedin { get; set; }
        [MaxLength(75)]
        public string Instagram { get; set; }
        [MaxLength(75)]
        public string Facebook { get; set; }
        [MaxLength(75)]
        public string Behance { get; set; }

    }
}
