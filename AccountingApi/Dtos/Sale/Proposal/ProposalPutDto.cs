using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Sale.Proposal
{
    public class ProposalPutDto
    {
        public string ProposalNumber { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? TotalPrice { get; set; }
        public double? Sum { get; set; }
        public double? TotalTax { get; set; }
        public string Desc { get; set; }

        public int? TaxId { get; set; }

        public int? ContragentId { get; set; }

        //public ICollection<ProposalItemPostDto> ProposalItemPostDtos { get; set; }

        //public ProposalPutDto()
        //{
        //    ProposalItemPostDtos = new Collection<ProposalItemPostDto>();
        //}

    }
}
