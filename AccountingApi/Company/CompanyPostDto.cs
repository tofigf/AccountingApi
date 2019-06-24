using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Company
{
    public class CompanyPostDto
    {
        public string PhotoFile { get; set; }
        public string CompanyName { get; set; }

        [MaxLength(75)]
        public string Name { get; set; }

        [MaxLength(75)]
        public string Surname { get; set; }

        [MaxLength(75)]
        public string Postion { get; set; }

        public bool IsCompany { get; set; } = true;

        [MaxLength(75)]
        public string FieldOfActivity { get; set; }

    }
}
