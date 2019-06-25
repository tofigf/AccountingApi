using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Dtos.Company;
using AccountingApi.Dtos.Sale.Proposal;
using AccountingApi.Helpers.Extentions;
using AccountingApi.Models;
using AccountingApi.Models.ViewModel;
using AutoMapper;
using EOfficeAPI.Helpers.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingApi.Controllers.V1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalController : ControllerBase
    {
        private readonly ISaleRepository _repo;
        private readonly IMapper _mapper;

        public ProposalController(ISaleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [Authorize]
        //Post [baseUrl]/api/proposal/addproposal
        [HttpPost]
        [Route("addproposal")]
        public async Task<IActionResult> AddProposal(VwProposal proposal, [FromHeader]int? companyId)
        {
            var mappedProposal = _mapper.Map<Proposal>(proposal.ProposalPost);

            var itemProposal = _mapper.Map<IEnumerable<ProposalItem>>(proposal.ProposalItemPosts);
            //Check

            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (mappedProposal == null)
                return StatusCode(409, "[Header]companyId or proposalId not correct");
            if (await _repo.CheckProposal(currentUserId, companyId))
                return Unauthorized();
            //mehsulun id-sinin olmasini yoxlayiriq.
            if (await _repo.CheckProposalProductId(itemProposal))
                return StatusCode(409, "productId doesnt exist");
            //contrageti id-ni yoxluyuruq
            if ((await _repo.CheckContragentId(mappedProposal.ContragentId, companyId)))
                return StatusCode(409, "contragetnId doesnt exist");
            #endregion

            var proposalToReturn = await _repo.CreateProposal(mappedProposal, companyId);

            var itemToRetun = await _repo.CreateProposalItems(itemProposal, mappedProposal.Id);

            var ToReturn = _mapper.Map<Dtos.Sale.Proposal.ProposalGetDto>(proposalToReturn);

            //Company
            //id ye gore sirketi getiririk
            Company companyFromRepo = await _repo.GetEditCompany(companyId);
            //// //map edirik
            Company companyForUpdate = _mapper.Map(proposal.CompanyPutProposalDto, companyFromRepo);
            //// //edit etmek reposu
            Company updatedCompany = await _repo.EditCompany(companyForUpdate, companyId, currentUserId);

            //Contragent
            //id-ye gore sirketi getiririk
            Contragent contragentFromRepo = await _repo.GetEditContragent(mappedProposal.ContragentId);
            //map edirik
            Contragent contragentForUpdate = _mapper.Map(proposal.ContragentPutInProposalDto, contragentFromRepo);
            //edit etmek reposu
            Contragent updatedContragent = await _repo.EditContragent(contragentForUpdate, companyId);

            //qaytaracagimiz info
            CompanyAfterPutDto companyToReturn = _mapper.Map<CompanyAfterPutDto>(updatedCompany);

            return Ok(ToReturn);

        }
        [Authorize]
        //Get[baseUrl]/api/proposal/getproposal
        [HttpGet]
        [Route("getproposal")]
        public async Task<IActionResult> GetProposal([FromQuery]PaginationParam productParam, [FromHeader]int? companyId)
        {
            //Yoxlamaq
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckProposal(currentUserId, companyId))
                return Unauthorized();

            #endregion

            var producstock = await _repo.GetProposal(productParam, companyId);

            var productsToReturn = _mapper.Map<IEnumerable<ProposalGetDto>>(producstock);
            return Ok(productsToReturn);
        }
        [Authorize]
        //Get[baseUrl]/api/proposal/geteditproposal
        [HttpGet]
        [Route("geteditproposal")]
        public async Task<IActionResult> GetEditProposal([FromHeader]int? proposalId, [FromHeader]int? companyId)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (proposalId == null)
                return StatusCode(409, "proposalId null");
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckProposal(currentUserId, companyId))
                return Unauthorized();
            if (await _repo.CheckProposalId(proposalId, companyId))
                return StatusCode(406, "Not Acceptable");
            #endregion
            Company company = await _repo.GetEditCompany(companyId);

            Contragent contragent = await _repo.GetContragentProposal(companyId, proposalId);

            Proposal fromRepo = await _repo.GetDetailProposal(proposalId, companyId);

            var ToReturn = _mapper.MergeInto<ProposalEditGetDto>(contragent, company, fromRepo);

            return Ok(ToReturn);
        }
        [Authorize]
        //Put [baseUrl]/api/proposal/updateproposal
        [HttpPut]
        [Route("updateproposal")]
        public async Task<IActionResult> UpdateProposal([FromBody] VwProposalPut proposalPut, [FromHeader]int? proposalId, [FromHeader]int? companyId)
        {
            //get proposal  for updating
            Proposal proposalfromRepo = await _repo.GetEditProposal(proposalId, companyId);
            //get proposalitems  for updating
            List<ProposalItem> proposalItemsFromRepo = await _repo.GetEditProposalItem(proposalId);
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (proposalId == null)
                return StatusCode(409, "proposalId null");
            if (currentUserId == null)
                return Unauthorized();
            if (proposalfromRepo == null)
                return StatusCode(409, "[Header]companyId or proposalId not correct");
            if (proposalItemsFromRepo == null)
                return StatusCode(409);
            if (await _repo.CheckProposal(currentUserId, companyId))
                return Unauthorized();

            #endregion
            //Mapping proposal  dto with proposalfromRepo
            Proposal Mapped = _mapper.Map(proposalPut.ProposalPutDto, proposalfromRepo);
            //Mapping proposalitems  dto with proposalItemsFromRepo
            List<ProposalItem> MapperdProposalItems = _mapper.Map(proposalPut.ProposalItemPutDtos, proposalItemsFromRepo);
            //Company
            //id ye gore sirketi getiririk
            Company companyFromRepo = await _repo.GetEditCompany(companyId);
            //map edirik
            Company companyForUpdate = _mapper.Map(proposalPut.CompanyPutProposalDto, companyFromRepo);
            //edit etmek reposu
            Company updatedCompany = await _repo.EditCompany(companyForUpdate, companyId, currentUserId);

            //Contragent
            //idye goe sirketi getiririk
            Contragent contragentFromRepo = await _repo.GetEditContragent(Mapped.ContragentId);
            //map edirik
            Contragent contragentForUpdate = _mapper.Map(proposalPut.ContragentPutInProposalDto, contragentFromRepo);
            //edit etmek reposu
            Contragent updatedContragent = await _repo.EditContragent(contragentForUpdate, companyId);
            //qaytaracagimiz info
            CompanyAfterPutDto companyToReturn = _mapper.Map<CompanyAfterPutDto>(updatedCompany);

            Proposal proposal = await _repo.EditProposal(Mapped, MapperdProposalItems, proposalId);

            return Ok(proposal);
        }
        [Authorize]
        //Post [baseUrl]/api/proposal/sendproposal
        [HttpPost]
        [Route("sendproposal")]
        public IActionResult SendProposal([FromHeader]int? proposalId, Mail mail)
        {
            if (proposalId == null)
                return StatusCode(409, "proposalId null");
            return Ok(_repo.CreateProposalSentMail(proposalId, mail.Email).Token);
        }
        //Get [baseUrl]/api/proposal/getproposalbytoken
        [HttpGet]
        [Route("getproposalbytoken")]
        public async Task<IActionResult> GetProposalByToken(string token, [FromHeader]int? proposalId, [FromHeader]int? companyId)
        {
            //Checking
            #region Check

            if (proposalId == null)
                return StatusCode(409, "proposalId null");
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (await _repo.CheckProposalId(proposalId, companyId))
                return StatusCode(406, "Not Acceptable");
            #endregion
            Company company = await _repo.GetEditCompany(companyId);

            Contragent contragent = await _repo.GetContragentProposal(companyId, proposalId);

            Proposal proposalFromRepo = _repo.GetProposalByToken(token);

            var ToReturn = _mapper.MergeInto<ProposalEditGetDto>(contragent, company, proposalFromRepo);

            return Ok(ToReturn);
        }
        //Delete [baseUrl]/api/proposal/deleteproposalitem
        [HttpDelete]
        [Route("deleteproposalitem")]
        public async Task<IActionResult> DeleteProposalItem([FromHeader] int? proposalItemId)
        {
            //Check
            #region Check
            if (proposalItemId == null)
                return StatusCode(409, "incomeItemId null");
            #endregion
            await _repo.DeleteProposalItem(proposalItemId);
            return Ok();
        }
        //Delete [baseUrl]/api/proposal/deleteproposal
        [HttpDelete]
        [Route("deleteproposal")]
        public async Task<IActionResult> DeleteProposal([FromHeader]int? companyId, [FromHeader] int? proposalId)
        {

            //Check
            #region Check
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (proposalId == null)
                return StatusCode(409, "proposalId null");
            #endregion
            await _repo.DeleteProposal(companyId, proposalId);
            return Ok();
        }
    }
}