using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}