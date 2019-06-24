﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Sale.Proposal
{
    public class ProposalItemPostDto
    {
        public int? Qty { get; set; }

        public double? SellPrice { get; set; }

        public double? TotalOneProduct { get; set; }

        public int? ProductId { get; set; }

        public int? ProposalId { get; set; }

    }
}
