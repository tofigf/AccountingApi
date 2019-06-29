using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Dtos.Company;
using AccountingApi.Dtos.Sale.Invoice;
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
  
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ISaleRepository _repo;
        private readonly ISettingRepository _repoSetting;
        private readonly IMapper _mapper;

        public InvoiceController(ISaleRepository repo, ISettingRepository settingRepo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _repoSetting = settingRepo;
        }
        //Post [baseUrl]/api/invoice/addinvoice
        [Authorize]
        [HttpPost]
        [Route("addinvoice")]
        public async Task<IActionResult> AddInvoice(VwInvoice invoice, [FromHeader] int? companyId)
        {

            //mapping for creating invoice
            var mappedInvoice = _mapper.Map<Invoice>(invoice.InvoicePostDto);

            //mapping for creating invoiceitems
            var mappeditemInvoice = _mapper.Map<List<InvoiceItem>>(invoice.InvoiceItemPosts);

            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (mappedInvoice == null)
                return StatusCode(409, "[Header]companyId or invoiceId not correct");
            if (await _repo.CheckInvoice(currentUserId, companyId))
                return Unauthorized();
            //cheking product id
            if (await _repo.CheckInvoiceProductId(mappeditemInvoice))
                return StatusCode(409, "productId doesnt exist");
            if (mappedInvoice.ContragentId == null)
                return StatusCode(409, "contragentId null");
            if (await _repo.CheckContragentId(mappedInvoice.ContragentId, companyId))
                return StatusCode(409, "contragentId doesnt exist");
            if (_repo.CheckInvoiceNegativeValue(mappedInvoice, mappeditemInvoice))
                return StatusCode(428, "negative value is detected");

            #endregion

            //creating repo 
            var invoiceToReturn = await _repo.CreateInvoice(mappedInvoice, companyId);
            //creating repo 
            var itemToRetun = await _repo.CreateInvoiceItems(mappeditemInvoice, mappedInvoice.Id);
            //mapping for return
            var ToReturn = _mapper.Map<InvoiceGetDto>(invoiceToReturn);

            //Company
            //id ye gore sirketi getiririk
            Company companyFromRepo = await _repo.GetEditCompany(companyId);
            //map edirik
            Company companyForUpdate = _mapper.Map(invoice.CompanyPutProposalDto, companyFromRepo);
            //edit etmek reposu
            Company updatedCompany = await _repo.EditCompany(companyForUpdate, companyId, currentUserId);

            //Contragent
            //idye goe sirketi getiririk
            Contragent contragentFromRepo = await _repo.GetEditContragent(mappedInvoice.ContragentId);
            //map edirik
            Contragent contragentForUpdate = _mapper.Map(invoice.ContragentPutInProposalDto, contragentFromRepo);
            //edit etmek reposu
            Contragent updatedContragent = await _repo.EditContragent(contragentForUpdate, companyId);
            //qaytaracagimiz info
            CompanyAfterPutDto companyToReturn = _mapper.Map<CompanyAfterPutDto>(updatedCompany);

            return Ok(ToReturn);
        }
        [Authorize]
        //Get [baseUrl]/api/invoice/getinvoice
        [HttpGet]
        [Route("getinvoice")]
        public async Task<IActionResult> GetInvoice([FromQuery]PaginationParam productParam, [FromHeader]int? companyId)
        {
            //Checking
            #region Check

            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckInvoice(currentUserId, companyId))
                return Unauthorized();
            #endregion
            //Repo Get
            var invoices = await _repo.GetInvoice(productParam, companyId);
            //Mapped object
            var invoiceToReturn = _mapper.Map<IEnumerable<InvoiceGetDto>>(invoices);

            return Ok(invoiceToReturn);
        }
        [Authorize]
        //Get [baseUrl]/api/invoice/geteditinvoice
        [HttpGet]
        [Route("geteditinvoice")]
        public async Task<IActionResult> GetEditInvoice([FromHeader]int? invoiceId, [FromHeader]int? companyId)
        {
            //Checking
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (invoiceId == null)
                return StatusCode(409, "InvoiceId null");
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckInvoice(currentUserId, companyId))
                return Unauthorized();
            if (await _repo.CheckInvoiceId(invoiceId, companyId))
                return StatusCode(406, "Not Acceptable");
            #endregion
            //get company by id
            Company company = await _repo.GetEditCompany(companyId);
            //get contragent by invoce id 
            Contragent contragent = await _repo.GetContragentInvoice(companyId, invoiceId);
            //get invoice by id
            Invoice fromRepo = await _repo.GetDetailInvoice(invoiceId, companyId);
            Tax tax = await _repoSetting.GetEditTax(companyId, fromRepo.TaxId);
            //mapping merge 3 table 
            var ToReturn = _mapper.MergeInto<InvoiceEditGetDto>(contragent, company, fromRepo, tax);

            return Ok(ToReturn);
        }
        [Authorize]
        //Put [baseUrl]/api/invoice/updateinvoice
        [HttpPut]
        [Route("updateinvoice")]
        public async Task<IActionResult> UpdateInvoice([FromBody] VwInvoicePut invoicePut, [FromHeader]int? invoiceId, [FromHeader]int? companyId)
        {
            Invoice fromRepo = await _repo.GetEditInvoice(invoiceId, companyId);
            List<InvoiceItem> invoiceItemsFromRepo = await _repo.GetEditInvoiceItem(invoiceId);

            Invoice Mapped = _mapper.Map(invoicePut.InvoicePutDto, fromRepo);
            List<InvoiceItem> MapperdInvoiceItems = _mapper.Map(invoicePut.InvoiceItemPutDtos, invoiceItemsFromRepo);
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (fromRepo == null)
                return StatusCode(409, "[Header]companyId or invoiceId not correct");
            if (await _repo.CheckInvoice(currentUserId, companyId))
                return StatusCode(409, "company id not correct");
            if (await _repo.CheckInvoiceId(invoiceId, companyId))
                return StatusCode(409, "invoiceId doesnt exist");
            if (_repo.CheckInvoiceNegativeValue(Mapped, MapperdInvoiceItems))
                return StatusCode(428, "negative value is detected");

            //cheking product id
            //if (await _repo.CheckInvoiceProductId(Mapped.InvoiceItems))
            //    return StatusCode(409, "productId doesnt exist");
            #endregion
            //Accounting
            var UpdateAccountDebit = _repo.UpdateInvoiceAccountDebit(invoiceId, companyId, invoicePut.InvoicePutDto, fromRepo.AccountDebitId);
            var UpdateAccountKredit = _repo.UpdateInvoiceAccountKredit(invoiceId, companyId, invoicePut.InvoicePutDto, fromRepo.AccountKreditId);


            //Check
            if (await _repo.CheckInvoiceItem(invoiceId, MapperdInvoiceItems))
                return StatusCode(409, "invoiceItem null");
            //Company Contragent Edit
            #region Company Contragent Edit
            // Company
            //  id ye gore sirketi getiririk
            Company companyFromRepo = await _repo.GetEditCompany(companyId);
            //map edirik
            Company companyForUpdate = _mapper.Map(invoicePut.CompanyPutProposalDto, companyFromRepo);
            //edit etmek reposu
            Company updatedCompany = await _repo.EditCompany(companyForUpdate, companyId, currentUserId);

            //Contragent
            //idye goe sirketi getiririk
            Contragent contragentFromRepo = await _repo.GetEditContragent(fromRepo.ContragentId);
            //map edirik
            Contragent contragentForUpdate = _mapper.Map(invoicePut.ContragentPutInProposalDto, contragentFromRepo);
            //edit etmek reposu
            Contragent updatedContragent = await _repo.EditContragent(contragentForUpdate, companyId);
            //qaytaracagimiz info
            CompanyAfterPutDto companyToReturn = _mapper.Map<CompanyAfterPutDto>(updatedCompany);

            #endregion

            Invoice invoice = await _repo.EditInvoice(Mapped, MapperdInvoiceItems, invoiceId);

            return Ok();
        }
        [Authorize]
        //Post [baseUrl]/api/invoice/sendinvoice
        [HttpPost]
        [Route("sendinvoice")]
        public IActionResult SendInvoice([FromHeader]int? invoiceId, Mail mail)
        {
            if (invoiceId == null)
                return StatusCode(409, "invoiceId null");
            return Ok(_repo.CreateInvoiceSentMail(invoiceId, mail.Email).Token);
        }
        //Get [baseUrl]/api/invoice/getinvoicebytoken
        [HttpGet]
        [Route("getinvoicebytoken")]
        public async Task<IActionResult> GetInvoiceByToken(string token, [FromHeader]int? invoiceId, [FromHeader]int? companyId)
        {
            //Checking
            #region Check

            if (invoiceId == null)
                return StatusCode(409, "invoiceId null");
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (await _repo.CheckProposalId(invoiceId, companyId))
                return StatusCode(406, "Not Acceptable");
            #endregion
            Company company = await _repo.GetEditCompany(companyId);

            Contragent contragent = await _repo.GetContragentInvoice(companyId, invoiceId);

            Invoice invoiceFromRepo = _repo.GetInvoiceByToken(token);

            var ToReturn = _mapper.MergeInto<InvoiceEditGetDto>(contragent, company, invoiceFromRepo);

            return Ok(ToReturn);
        }
        [Authorize]
        //Get[baseUrl]/api/invoice/existincome
       [HttpGet]
       [Route("existincome")]
        public async Task<bool> ExistIncome([FromHeader] int? invoiceId)
        {
            if (await _repo.CheckExistIncomeByInvoiceId(invoiceId))
                return true;

            return false;
        }
        //Delete [baseUrl]/api/invoice/deleteinvoiceitem
        [HttpDelete]
        [Route("deleteinvoiceitem")]
        public async Task<IActionResult> DeleteInvoiceItem([FromHeader] int? invoiceItemId)
        {
            //Check
            #region Check
            if (invoiceItemId == null)
                return StatusCode(409, "invoiceItemId null");
            //if (await _repo.DeleteInvoiceItem(invoiceItemId) == null)
            //    return NotFound();
            #endregion
            await _repo.DeleteInvoiceItem(invoiceItemId);
            return Ok();
        }
        //Delete [baseUrl]/api/invoice/deleteinvoice
        [HttpDelete]
        [Route("deleteinvoice")]
        public async Task<IActionResult> DeleteInvoice([FromHeader] int? companyId, [FromHeader] int? invoiceId)
        {
            //Check
            #region Check
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (invoiceId == null)
                return StatusCode(409, "invoiceId null");
            if (await _repo.DeleteInvoice(companyId, invoiceId) == null)
                return StatusCode(409, "object null");
            #endregion
            await _repo.DeleteInvoice(companyId, invoiceId);
            return Ok();
        }
    }
}