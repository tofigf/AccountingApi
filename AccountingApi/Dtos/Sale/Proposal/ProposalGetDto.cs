using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Sale.Proposal
{
    public class ProposalGetDto
    {
        public int Id { get; set; }
        public string ProposalNumber { get; set; }

        public DateTime? PreparingDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double? TotalPrice { get; set; }
        //contragent companyname
        public string ContragentCompanyName { get; set; }

        public byte IsPaid { get; set; }


    }
}
