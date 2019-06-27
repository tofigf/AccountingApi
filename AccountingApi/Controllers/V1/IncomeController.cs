using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Dtos.Sale.Income;
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
    public class IncomeController : ControllerBase
    {
        private readonly ISaleRepository _repo;
        private readonly IMapper _mapper;
        public IncomeController(ISaleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        //Get [baseUrl]/api/income/getinvoicebycontragentid
        [HttpGet]
        [Route("getinvoicebycontragentid")]
        public IActionResult GetInvoiceByContragentId([FromHeader]int? contragentId, [FromHeader] int? companyId)
        {
            if (contragentId == null)
                return StatusCode(409, "contragentId null");

            var invoice = _repo.GetInvoiceByContragentId(contragentId, companyId);

            var invoiceToReturn = _mapper.Map<IEnumerable<IncomeInvoiceGetDto>>(invoice);
            return Ok(invoice);
        }
        //Get [baseUrl]/api/income/getincome
        [HttpGet]
        [Route("getincome")]
        public async Task<IActionResult> GetIncome([FromQuery]PaginationParam productParam, [FromHeader]int? companyId)
        {
            //Checking
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckIncome(currentUserId, companyId))
                return Unauthorized();
            #endregion
            //Repo Get
            var invoices = await _repo.GetIncome(productParam, companyId);
            //Mapped object
            var invoiceToReturn = _mapper.Map<IEnumerable<IncomeGetDto>>(invoices);

            return Ok(invoices);
        }
        // get edit income
        //Get [baseUrl]/api/income/geteditincome    
        [HttpGet]
        [Route("geteditincome")]
        public async Task<IActionResult> GetEditIncome([FromHeader] int? companyId, [FromHeader] int? invoiceId)
        {
            //Repo Get
            //var incomeItemsinvoices = await _repo.GetEditAllIncomes(companyId,invoiceId);
            var incomeItemsinvoices = await _repo.GetInvoiceIcomeItem(companyId, invoiceId);
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckIncome(currentUserId, companyId))
                return Unauthorized();
            if (incomeItemsinvoices == null)
                return StatusCode(406, "content null");
            #endregion

            // Mapped object
            var ToReturn = _mapper.Map<IncomeInvoiceEditGetDto>(incomeItemsinvoices);

            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/income/getdetailincome    
        [HttpGet]
        [Route("getdetailincome")]
        public async Task<IActionResult> GetDetailIncome([FromHeader] int? companyId, [FromHeader] int? incomeId)
        {
            //Checking
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckIncome(currentUserId, companyId))
                return Unauthorized();
            #endregion

            var invoices = await _repo.DetailIncome(incomeId, companyId);
            // Mapped object
            var ToReturn = _mapper.Map<IncomeEditGetDto>(invoices);

            return Ok(ToReturn);
        }
        //Post [baseUrl]/api/income/createincome
        [HttpPost]
        [Route("createincome")]
        public async Task<IActionResult> CreateIncome([FromHeader] int? companyId, [FromHeader] int? contragentId, [FromBody]VwCreateIncome createIncome)
        {
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (contragentId == null)
                return StatusCode(409, "contragentId null");
            //mapped income
            var mappedIncome = _mapper.Map<Income>(createIncome.IncomePostDto);
            //mapped incomeitems
            var mappedIncomeItem = _mapper.Map<List<IncomeItem>>(createIncome.IncomeItemPostDtos);

            //Check:
            #region Check
            //checking incomeitems paidmoney big than invoice total price  
            if (await _repo.CheckIncomeEqualingInvoiceTotalPriceForCreate(mappedIncomeItem))
                return StatusCode(411, "paidmoney big than totalmoney or one invoiceId doesnt exist");
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckIncome(currentUserId, companyId))
                return Unauthorized();

            // id-sinin olmasini yoxlayiriq.
            if (await _repo.CheckIncomeContragentIdInvoiceId(contragentId, companyId))
                return StatusCode(409, "contragentId  doesnt exist");

            #endregion

            var FromRepoIncomes = await _repo.CreateIncome(companyId, contragentId, createIncome.Ids, mappedIncome, mappedIncomeItem);

            return Ok(FromRepoIncomes);
        }
        //Put [baseUrl]/api/income/updateincome
        [HttpPut]
        [Route("updateincome")]
        public async Task<IActionResult> UpdateIncome(VwIncomePut incomePut, [FromHeader] int? companyId, [FromHeader] int? incomeId, [FromHeader] int? invoiceId)
        {
            //Get edit income
            //didnt use yet
            Income fromRepo = await _repo.GetEditIncome(incomeId, companyId);
            //Get edit incomeitems
            List<IncomeItem> incomeItems = await _repo.GetEditIncomeItems(invoiceId);
            //Checking
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckIncome(currentUserId, companyId))
                return Unauthorized();
            //checkig totalpaidmoney and totaloneinvoice

            #endregion

            //mapping income
            Income Mapped = _mapper.Map(incomePut.IncomePutDto, fromRepo);

            //mapping incomeitems
            List<IncomeItem> incomeItemsMapped = _mapper.Map(incomePut.IncomeItemGetEditDtos, incomeItems);

            if (await _repo.CheckIncomeEqualingInvoiceTotalPriceForUpdate(incomeItemsMapped, invoiceId))
                return StatusCode(411, "paidmoney big than totalmoney or that invoice  doesn't exist");
            //Put income and inomeitems
            var income = await _repo.EditIncome(incomeItemsMapped, invoiceId);

            return Ok();

        }
        //Delete [baseUrl]/api/income/deleteincomeitem
        [HttpDelete]
        [Route("deleteincomeitem")]
        public async Task<IActionResult> DeleteIncomeItem([FromHeader] int? incomeItemId)
        {

            if (incomeItemId == null)
                return StatusCode(409, "incomeItemId null");
            await _repo.DeleteIncomeItem(incomeItemId);

            return Ok();
        }
    }
}