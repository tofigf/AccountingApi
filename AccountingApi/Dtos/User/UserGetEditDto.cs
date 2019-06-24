using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.User
{
    public class UserGetEditDto
    {
        public string Name { get; set; }
        [MaxLength(100)]
        public string SurName { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }
       
        //public string Password { get; set; }
    }
}
