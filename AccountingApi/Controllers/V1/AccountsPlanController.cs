using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingApi.Controllers.V1
{
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
    }
}