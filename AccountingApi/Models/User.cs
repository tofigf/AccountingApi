using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(75)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string SurName { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(150)]
        public string Token { get; set; }
        public bool IsDeleted { get; set; } = false;

        public bool Status { get; set; } = true;

        [MaxLength(150)]
        public string Password { get; set; }

        public ICollection<Company> Companies { get; set; }
    }
}
