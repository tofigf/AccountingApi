using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Sale.Proposal
{
    public class ProposalPostDto
    {
        public string CompanyVoen { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string FileUrl { get; set; }
        public string ContragentVoen { get; set; }
        public string ProposalNumber { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? TotalPrice { get; set; }
        public double? Sum { get; set; }
        public double? TotalTax { get; set; }

        public string Desc { get; set; }

        [Required, Range(1, 4)]
        public byte IsPaid { get; set; } = 1;

        public int? ContragentId { get; set; }

        public int CompanyId { get; set; }
        public int? TaxId { get; set; }


        public ICollection<ProposalItemGetDto> ProposalItemGetDtos { get; set; }

        public ProposalPostDto()
        {
            ProposalItemGetDtos = new Collection<ProposalItemGetDto>();
        }

    }
}
