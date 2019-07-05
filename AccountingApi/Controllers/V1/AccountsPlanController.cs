using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Dtos.AccountsPlan;
using AccountingApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingApi.Controllers.V1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsPlanController : ControllerBase
    {
        private readonly IAccountsPlanRepository _repo;
        private readonly IMapper _mapper;

        public AccountsPlanController(IAccountsPlanRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        //Get [baseUrl]/api/accountsplan/accountplanimportfromexcel
        [HttpGet]
        [Route("accountplanimportfromexcel")]
        public async Task<IActionResult> AccountPlanImportFromExcel([FromHeader] int? companyId)
        {
            if (companyId == null)
                return StatusCode(409, "companyId null");
           await   _repo.ImportFromExcel(companyId);

            return Ok();
        }

        //Get [baseUrl]/api/accountsplan/getaccountsplan
        [HttpGet]
        [Route("getaccountsplan")]
        public async Task<IActionResult> GetAccountsPlan([FromHeader] int? companyId)
        {
            if (companyId == null)
                return StatusCode(409, "companyId null");
             List<AccountsPlan> accounts  =  await  _repo.GetAccountsPlans(companyId);

            if (accounts == null)
                return StatusCode(409, "object null");
            var accountsToReturn = _mapper.Map<IEnumerable<AccountsPlanGetDto>>(accounts);


            return Ok (accountsToReturn);
        }

        //Get [baseUrl]/api/accountsplan/balancesheet
        [HttpGet]
        [Route("balancesheet")]
        public async Task<IActionResult> BalanceSheet([FromHeader] int? companyId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (companyId == null)
                return StatusCode(409, "companyId null");

            var balance = await _repo.BalanceSheet(companyId, startDate, endDate);
            if(balance == null)
            {
                return StatusCode(409,"object null");
            }
            return Ok(balance);
        }

        //ManualJournal
        #region ManualJournal
        //Get [baseUrl]/api/accountsplan/getoperationcategory
        [HttpGet]
        [Route("getoperationcategory")]
        public async Task<IActionResult> GetOperationCategory()
        {
            return Ok(await _repo.GetOperationCategories());
        }
        //Post [baseUrl]/api/accountsplan/createmanualjournal
        [HttpPost]
        [Route("createmanualjournal")]
        public async Task<IActionResult> CreateManualJournal([FromHeader] int? companyId, ManualJournalPostDto journalPostDto)
        {
            //mapping for creating
            var mappedJournal = _mapper.Map<ManualJournal>(journalPostDto);

            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            #endregion

            //creating repo 
            ManualJournal ToReturn = await _repo.CreateManualJournal(companyId, mappedJournal);

            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/accountsplan/getmanualjournal
        [HttpGet]
        [Route("getmanualjournal")]
        public async Task<IActionResult> GetManualJournal([FromHeader] int? companyId)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            #endregion
            var ManualJournalFromRepo =   await  _repo.GetManualJournals(companyId);

            var ToReturn = _mapper.Map<IEnumerable<ManualJournalGetDto>>(ManualJournalFromRepo);

            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/accountsplan/geteditmanualjournal
        [HttpGet]
        [Route("geteditmanualjournal")]
        public async Task<IActionResult> GetEditManualJournal([FromHeader] int? companyId,[FromHeader] int? journalId)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            #endregion

            var editRepo =  await _repo.GetEditManualJournal(companyId, journalId);

            var ToReturn = _mapper.Map<ManualJournalGetEditDto>(editRepo);

            return Ok(ToReturn);
        }
        //Put [baseUrl]/api/accountsplan/editmanualjournal
        [HttpPut]
        [Route("editmanualjournal")]
        public async Task<IActionResult>EditManualJournal(ManualJournalPostDto journalPostDto ,[FromHeader]int? companyId,[FromHeader] int? journalId) 
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            #endregion

            var getEditRepo =  await  _repo.GetEditManualJournal(companyId, journalId);

            //Account:
            var UpdateAccountDebit = _repo.UpdateManualJournalAccountDebit(journalId, companyId, journalPostDto, getEditRepo.AccountDebitId);
            var UpdateAccountKredit = _repo.UpdateManualJournalAccountKredit(journalId, companyId, journalPostDto, getEditRepo.AccountKreditId);

            var Mapped = _mapper.Map(journalPostDto, getEditRepo);

             var edited =  await  _repo.EditManualJournal(Mapped);

            var  ToReturn = _mapper.Map<ManualJournalGetEditDto>(edited);

            return Ok(ToReturn);
        }
        //Delete [baseUrl]/api/accountsplan/deletemanualjournal
        [HttpDelete]
        [Route("deletemanualjournal")]
        public async Task<IActionResult> DeleteManualJournal([FromHeader] int? companyId, [FromHeader] int? journalId)
        {

            //Check
            #region Check
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (journalId == null)
                return StatusCode(409, "invoiceId null");
            if (await _repo.DeleteManualJournal(companyId, journalId) == null)
                return StatusCode(409, "object null");
            #endregion

            await _repo.DeleteManualJournal(companyId, journalId);
            return Ok();
        }
        #endregion

    }
}