using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models
{
    public class InvoiceSentMail
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Token { get; set; }
        public int InvoiceId { get; set; }
        [Required, Range(1, 4)]
        public byte IsPaid { get; set; } = 1;


        public virtual Invoice Invoice { get; set; }
    }
}
