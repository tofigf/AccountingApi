using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.User
{
    public class UserForReturnDto
    {
        public int Id { get; set; }
        
        public string Email { get; set; }

        //public string Token { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }


    }
}
