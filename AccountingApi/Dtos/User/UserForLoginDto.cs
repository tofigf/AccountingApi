using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.User
{
    public class UserForLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
