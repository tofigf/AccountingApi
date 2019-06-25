using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Models;
using AccountingApi.Models.ViewModel;
using AutoMapper;
using EOfficeAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingRepository _repo;
        private readonly IMapper _mapper;

        public SettingController(ISettingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        //Get [baseUrl]/api/setting/getunits
        [HttpGet]
        [Route("getunits")]
        public async Task<IActionResult> GetUnits([FromHeader]int? companyId)
        {
            //Yoxlamaq
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckUnit(currentUserId, companyId))
                return Unauthorized();

            var fromRepo = await _repo.GetProduct_Units(companyId);

            return Ok(fromRepo);
        }
        //Get [baseUrl]/api/setting/gettaxes
        [HttpGet]
        [Route("gettaxes")]
        public async Task<IActionResult> GetTaxes([FromHeader]int? companyId)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckTax(currentUserId, companyId))
                return Unauthorized();
            #endregion

            var fromRepo = await _repo.GetTaxs(companyId);

            return Ok(fromRepo);
        }
        //Get [baseUrl]/api/setting/getedittax
        [HttpGet]
        [Route("getedittax")]
        public async Task<IActionResult> GetEditTax([FromHeader]int? companyId, [FromHeader] int? taxId)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (taxId == null)
                return null;
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckTax(currentUserId, companyId))
                return Unauthorized();
            #endregion

            return Ok(await _repo.GetEditTax(companyId, taxId));
        }
        //Post [baseUrl]/api/setting/sendmail
        [HttpPost]
        [Route("sendmail")]
        public IActionResult SendMail(Mail email)
        {
            MailExtention.Send(email.Subject, email.Body, email.Email);
            return NoContent();
        }
        //Post [baseUrl]/api/setting/createtax
        [HttpPost]
        [Route("createtax")]
        public async Task<IActionResult> CreateTax(Tax tax, [FromHeader] int? companyId)
        {
            var createdTax = await _repo.CreateTax(tax, companyId);

            return Ok(createdTax);
        }
        //Put [baseUrl]/api/setting/updatetax
        [HttpPut]
        [Route("updatetax")]
        public async Task<IActionResult> UpdateTax(Tax tax)
        {
            if (tax == null)
                return StatusCode(406, "content null");

            Tax updatedTax = await _repo.UpdateTax(tax);

            return Ok(updatedTax);
        }
        //Put [baseUrl]/api/setting/deletetax
        [HttpPut]
        [Route("deletetax")]
        public async Task<IActionResult> Deletetax([FromHeader] int? companyId, [FromHeader] int? taxId)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (taxId == null)
                return null;
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckTax(currentUserId, companyId))
                return Unauthorized();
            #endregion
            await _repo.DeleteTax(companyId, taxId);


            return Ok();
        }
    }
}