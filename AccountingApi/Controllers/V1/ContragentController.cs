using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Helpers.Extentions;
using AccountingApi.Models;
using AutoMapper;
using EOfficeAPI.Dtos.Nomenklatura.Kontragent;
using EOfficeAPI.Helpers.Extentions;
using EOfficeAPI.Helpers.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContragentController : ControllerBase
    {
        private readonly INomenklaturaRepository _repo;
        private readonly IMapper _mapper;
        public ContragentController(INomenklaturaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("addcontragent")]
        public async Task<IActionResult> AddContragent(ContragentPostDto vwContragent, [FromHeader]int? companyId)
        {
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckContragent(currentUserId, companyId))
                return Unauthorized();
            if (!ModelState.IsValid)
                return BadRequest();
            //workerpostdto-nu worker table-na mapp etmek
            Contragent contragent = _mapper.Map<Contragent>(vwContragent);
            //mapp olunmus workeri CreateWorker reposuna gonderirik.
            Contragent ContragentToGet = await _repo.CreateContragent(contragent, companyId);
            //database elave olunmus workeri qaytarmaq ucun 
            ContragentGetDto contragentForReturn = _mapper.Map<ContragentGetDto>(ContragentToGet);

            //Worker_Detail hissesi
            Contragent_Detail contragent_Details = _mapper.Map<Contragent_Detail>(vwContragent);

            Contragent_Detail contragent_DetailToGet = await _repo.CreateContragent_Detail(contragent_Details, contragent.Id);

            //ContragentGetDto contragent_DetailForReturn = _mapper.Map<ContragentGetDto>(contragent_DetailToGet);

            return Ok(contragentForReturn);
        }
        //Get [baseUrl]/api/contragent/getcontragents
        [HttpGet]
        [Route("getcontragents")]
        // pagination yazilmsdi                                                     
        public async Task<IActionResult> GetContragents([FromQuery]PaginationParam contragentParam, [FromHeader]int? companyId)
        {
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckContragent(currentUserId, companyId))
                return Unauthorized();

            var contragents = await _repo.GetContragents(contragentParam, companyId);

            var contragentToReturn = _mapper.Map<IEnumerable<ContragentGetDto>>(contragents);

            Response.AddPagination(contragents.CurrentPage, contragents.PageSize,
              contragents.TotalCount, contragents.TotalPages);

            return Ok(contragentToReturn);
        }
        //Get [baseUrl]/api/contragent/getsalecontragents
        //getsalecontragents (where added contragent selecting sale)
        [HttpGet]
        [Route("getsalecontragents")]
        // pagination yazilmsdi                                                      
        public async Task<IActionResult> GetSaleContragents([FromQuery]PaginationParam contragentParam, [FromHeader]int? companyId)
        {
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckContragent(currentUserId, companyId))
                return Unauthorized();

            var contragents = await _repo.GetSallerContragents(contragentParam, companyId);

            var contragentToReturn = _mapper.Map<IEnumerable<ContragentGetDto>>(contragents);

            Response.AddPagination(contragents.CurrentPage, contragents.PageSize,
              contragents.TotalCount, contragents.TotalPages);

            return Ok(contragentToReturn);
        }
        //Get [baseUrl]/api/contragent/getcostumercontragents
        //getcostumercontragents (where added contragent selecting customer)
        [HttpGet]
        [Route("getcostumercontragents")]
        // pagination yazilmsdi                                                      
        public async Task<IActionResult> GetCostumerContragents([FromQuery]PaginationParam contragentParam, [FromHeader]int? companyId)
        {
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckContragent(currentUserId, companyId))
                return Unauthorized();

            var contragents = await _repo.GetCostumerContragents(contragentParam, companyId);

            var contragentToReturn = _mapper.Map<IEnumerable<ContragentGetDto>>(contragents);

            Response.AddPagination(contragents.CurrentPage, contragents.PageSize,
              contragents.TotalCount, contragents.TotalPages);

            return Ok(contragentToReturn);
        }

        //deyisilik etmek ucun iscini getiririk
        [HttpGet]
        [Route("geteditcontragent")]
        public async Task<IActionResult> GetEditContragent([FromHeader]int? contragentId, [FromHeader]int? companyId)
        {
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (contragentId == null)
                return StatusCode(409, "contragentId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckContragent(currentUserId, companyId))
                return Unauthorized();
            if (await _repo.CheckContragentId(contragentId, companyId))
                return StatusCode(406, "Not Acceptable");

            Contragent contragent = await _repo.GetEditContragent(contragentId, companyId);

            Contragent_Detail contragent_Detail = await _repo.GetEditContragent_Detail(contragentId);

            //2 obyekti birlesdirmek
            var contragentToReturn = _mapper.MergeInto<ContragentGetEditDto>(contragent, contragent_Detail);

            return Ok(contragentToReturn);
        }
        //deyismek
        [HttpPut]
        [Route("updatecontragent")]
        //Update zamani bu metodla dolduracayiq
        public async Task<IActionResult> UpdateContragent([FromBody]ContragentPutDto contragentPut, [FromHeader]int? contragentId, [FromHeader] int? companyId)
        {
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (contragentId == null)
                return StatusCode(409, "contragentId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckContragent(currentUserId, companyId))
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest();
            //isciler
            //repoya id gonderirik ve o bize lazim olan iscini ve detail-ni getirir.
            Contragent contragent_FromRepo = await _repo.GetEditContragent(contragentId, companyId);

            Contragent_Detail Detail_FromRepo = await _repo.GetEditContragent_Detail(contragentId);

            //mapp olunmus 
            Contragent contragentMapped = _mapper.Map(contragentPut, contragent_FromRepo);
            //repoda  yenileyirik 
            Contragent updatedContragent = await _repo.EditContragent(contragent_FromRepo, contragentId);
            //qaytarmaq ucun 
            ContragentGetDto contragentToReturn = _mapper.Map<ContragentGetDto>(updatedContragent);
            //elave melumatlari
            //mapp olunmus elaqlerin detaili
            Contragent_Detail contragent_DetailToMapped = _mapper.Map(contragentPut, Detail_FromRepo);
            //repoda iscini yenileyirik 
            Contragent_Detail updatedContragent_Detail = await _repo.EditContragent_Detail(Detail_FromRepo, contragentId);

            return Ok(contragentToReturn);
        }

        //Delete [baseUrl]/api/contragent/deletecontragent
        [HttpGet]
        [Route("deletecontragent")]
        public async Task<IActionResult> DeleteContragent([FromHeader]int? contragentId, [FromHeader]int? companyId)
        {
            //Check
            #region Check
            if (contragentId == null)
                return StatusCode(409, "workerId null");
            if (companyId == null)
                return StatusCode(409, "companyId null");

            if (await _repo.DeleteContragent(contragentId, companyId) == null)
                return NotFound();
            #endregion

            Contragent DeletedContragent = await _repo.DeleteContragent(contragentId, companyId);

            return Ok();
        }
    }
}