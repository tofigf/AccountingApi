using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class Contragent_Detail
    {
        [Key]
        public int Id { get; set; }
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

        public int ContragentId { get; set; }

        public Contragent Contragent { get; set; }
    }
}
