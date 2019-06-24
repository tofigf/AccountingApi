using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.User
{
    public class UserPutDto
    {
        //public string Name { get; set; }
        //public string SurName { get; set; }
        // public string Email { get; set; }
        public string OldPassword { get; set; }

        public string Password { get; set; }
    }
}
