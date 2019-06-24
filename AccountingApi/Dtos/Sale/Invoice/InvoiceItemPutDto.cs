using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EOfficeAPI.Dtos.Sale.Invoice
{
    public class InvoiceItemPutDto
    {
        public int Id { get; set; }

        public int? Qty { get; set; }

        public double? SellPrice { get; set; }

        public double? TotalOneProduct { get; set; }

        public int? ProductId { get; set; }

    }
}
