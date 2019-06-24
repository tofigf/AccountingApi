using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Dtos.Sale.Proposal
{
    public class ProposalEditGetDto
    {
        public string ProposalNumber { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalTax { get; set; }
        public double? Sum { get; set; }
        public double? TaxRate { get; set; }
        public int? TaxId { get; set; }
        public int? ContragentId { get; set; }
        //company
        public string CompanyCompanyName { get; set; }
        public string CompanyVoen { get; set; }
        //contragent
        public string ContragentCompanyName { get; set; }
        public string ContragentVoen { get; set; }
        public byte IsPaid { get; set; }

        public ICollection<ProposalItemGetDto> ProposalItemGetDtos { get; set; }

        public ProposalEditGetDto()
        {
            ProposalItemGetDtos = new Collection<ProposalItemGetDto>();
        }
    }
}
