using AccountingApi.Dtos.Company;
using AccountingApi.Dtos.Nomenklatura.Kontragent;
using AccountingApi.Dtos.Sale.Proposal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Models.ViewModel
{
    public class VwProposal
    {
        public ProposalPostDto ProposalPost { get; set; }

        public ICollection<ProposalItemPostDto> ProposalItemPosts { get; set; }

        public CompanyPutProposalDto CompanyPutProposalDto { get; set; }

        public ContragentPutInProposalDto ContragentPutInProposalDto { get; set; }
    }
}
