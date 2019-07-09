using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Dtos.Company;
using AccountingApi.Dtos.Purchase.Expense;
using AccountingApi.Dtos.Purchase.ExpenseInvoice;
using AccountingApi.Helpers.Extentions;
using AccountingApi.Models;
using AccountingApi.Models.ViewModel;
using AutoMapper;
using EOfficeAPI.Helpers.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOfficeAPI.Controllers.V1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IPurchaseRepository _repo;
        private readonly ISaleRepository _saleRepo;
        private readonly IMapper _mapper;

        public ExpenseController(IPurchaseRepository repo, ISaleRepository saleRepo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _saleRepo = saleRepo;
        }

        //ExpenseInvoice
        #region ExpenseInvoice
        //Post [baseUrl]/api/expense/addexpenseinvoice
        [HttpPost]
        [Route("addexpenseinvoice")]
        public async Task<IActionResult> AddExpenseInvoice(VwCreateExpenseInvoice expenseInvoice, [FromHeader]int? companyId)
        {
            //mapping for creating invoice
            var mappedInvoice = _mapper.Map<ExpenseInvoice>(expenseInvoice.ExpenseInvoicePostDto);
            //mapping for creating invoiceitems
            var mappeditemInvoice = _mapper.Map<List<ExpenseInvoiceItem>>(expenseInvoice.ExpenseInvoiceItemPostDtos);
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckExpenseInvoice(currentUserId, companyId))
                return Unauthorized();
            //cheking product id
            if (await _repo.CheckExpenseInvoiceProductId(mappeditemInvoice))
                return StatusCode(409, "productId doesnt exist");
            if (mappedInvoice.ContragentId == null)
                return StatusCode(409, "contragentId null");
            if (await _saleRepo.CheckContragentId(mappedInvoice.ContragentId, companyId))
                return StatusCode(409, "contragentId doesnt exist");
            if (_repo.CheckInvoiceNegativeValue(mappedInvoice, mappeditemInvoice))
                return StatusCode(428, "negative value is detected");
            #endregion
            //creating repo 
            var invoiceToReturn = await _repo.CreateInvoice(mappedInvoice, companyId);
            //creating repo 
            var itemToRetun = await _repo.CreateInvoiceItems(mappeditemInvoice, mappedInvoice.Id);
            //mapping for return
            var ToReturn = _mapper.Map<ExpenseInvoiceGetDto>(invoiceToReturn);

            //Company
            //id ye gore sirketi getiririk
            Company companyFromRepo = await _saleRepo.GetEditCompany(companyId);
            //map edirik
            Company companyForUpdate = _mapper.Map(expenseInvoice.CompanyPutProposalDto, companyFromRepo);
            //edit etmek reposu
            Company updatedCompany = await _saleRepo.EditCompany(companyForUpdate, companyId, currentUserId);
            //Contragent
            //idye goe sirketi getiririk
            Contragent contragentFromRepo = await _saleRepo.GetEditContragent(companyId);
            //map edirik
            Contragent contragentForUpdate = _mapper.Map(expenseInvoice.ContragentPutInProposalDto, contragentFromRepo);
            //edit etmek reposu
            Contragent updatedContragent = await _saleRepo.EditContragent(contragentForUpdate, companyId);
            //qaytaracagimiz info
            CompanyAfterPutDto companyToReturn = _mapper.Map<CompanyAfterPutDto>(updatedCompany);

            return Ok();
        }
        //Get [baseUrl]/api/expense/getexpensiveinvoice
        [HttpGet]
        [Route("getexpensiveinvoice")]
        public async Task<IActionResult> GetExpensiveInvoice([FromQuery]PaginationParam productParam, [FromHeader]int? companyId)
        {
            //Checking
            #region Check

            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckExpenseInvoice(currentUserId, companyId))
                return Unauthorized();

            #endregion
            //Repo Get
            var invoices = await _repo.GetInvoice(productParam, companyId);
            //Mapped object
            var invoiceToReturn = _mapper.Map<IEnumerable<ExpenseInvoiceGetDto>>(invoices);

            return Ok(invoiceToReturn);
        }
        //Get [baseUrl]/api/expense/geteditexpenseinvoice
        [HttpGet]
        [Route("geteditexpenseinvoice")]
        public async Task<IActionResult> GetEditExpenseInvoice([FromHeader] int? invoiceId, [FromHeader] int? companyId)
        {
            //Checking
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (invoiceId == null)
                return StatusCode(409, "invoiceId null");
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckExpenseInvoice(currentUserId, companyId))
                return Unauthorized();
            if (await _repo.CheckExpenseInvoiceId(invoiceId, companyId))
                return StatusCode(406, "Not Acceptable");
            #endregion
            //get company by id
            Company company = await _saleRepo.GetEditCompany(companyId);
            //get contragent by invoce id 
            Contragent contragent = await _repo.GetContragentInvoice(companyId, invoiceId);
            //get invoice by id
            ExpenseInvoice fromRepo = await _repo.GetDetailInvoice(invoiceId, companyId);
            //mapping merge 3 table 
            var ToReturn = _mapper.MergeInto<ExpensiveInvoiceEditGetDto>(contragent, company, fromRepo);

            return Ok(ToReturn);
        }
        //Put [baseUrl]/api/expense/updateexpenseinvoice
        [HttpPut]
        [Route("updateexpenseinvoice")]
        public async  Task<IActionResult> UpdateExpenseInvoice([FromBody] VwExpenseInvoicePutDto proposalPut, [FromHeader]int? invoiceId, [FromHeader]int? companyId)
        {
            ExpenseInvoice fromRepo = await _repo.GetEditInvoice(invoiceId, companyId);

            List<ExpenseInvoiceItem> invoiceItemsFromRepo = await _repo.GetEditInvoiceItem(invoiceId);

            ExpenseInvoice Mapped = _mapper.Map(proposalPut.ExpenseInvoicePutDto, fromRepo);

            List<ExpenseInvoiceItem> MapperdInvoiceItems = _mapper.Map(proposalPut.ExpenseInvoiceItemPutDtos, invoiceItemsFromRepo);

            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckExpenseInvoice(currentUserId, companyId))
                return Unauthorized();
            if (_repo.CheckInvoiceNegativeValue(Mapped, MapperdInvoiceItems))
                return StatusCode(428, "negative value is detected");
            if (await _repo.CheckExpenseInvoiceItem(invoiceId, MapperdInvoiceItems))
                return StatusCode(409, "invoiceItem not correct");
            //cheking product id
            //if (await _repo.CheckExpenseInvoiceProductId(mappeditemInvoice))
            //    return StatusCode(409, "productId doesnt exist");
            #endregion

            //Accounting
            var UpdateAccountDebit = _repo.UpdateInvoiceAccountDebit(invoiceId, companyId, proposalPut.ExpenseInvoicePutDto, fromRepo.AccountDebitId);
            var UpdateAccountKredit = _repo.UpdateInvoiceAccountKredit(invoiceId, companyId, proposalPut.ExpenseInvoicePutDto, fromRepo.AccountKreditId);

            //Company Contragent Edit
            #region Company Contragent Edit
            //Company
            //id ye gore sirketi getiririk
            Company companyFromRepo = await _saleRepo.GetEditCompany(companyId);
            //map edirik
            Company companyForUpdate = _mapper.Map(proposalPut.CompanyPutProposalDto, companyFromRepo);
            //edit etmek reposu
            Company updatedCompany = await _saleRepo.EditCompany(companyForUpdate, companyId, currentUserId);
            //Contragent
            //idye goe sirketi getiririk
            Contragent contragentFromRepo = await _saleRepo.GetEditContragent(companyId);
            //map edirik
            Contragent contragentForUpdate = _mapper.Map(proposalPut.ContragentPutInProposalDto, contragentFromRepo);
            //edit etmek reposu
            Contragent updatedContragent = await _saleRepo.EditContragent(contragentForUpdate, companyId);
            //qaytaracagimiz info
            CompanyAfterPutDto companyToReturn = _mapper.Map<CompanyAfterPutDto>(updatedCompany);

            #endregion

            ExpenseInvoice invoice = await _repo.EditInvoice(Mapped, MapperdInvoiceItems, invoiceId);

            return Ok();
        }
        //Delete [baseUrl]/api/expense/deleteexpenseinvoiceitem
        [HttpDelete]
        [Route("deleteexpenseinvoiceitem")]
        public async Task<IActionResult> DeleteExpenseInvoiceItem([FromHeader] int? expenseInvoiceItemId)
        {
            //Check
            #region Check
            if (expenseInvoiceItemId == null)
                return StatusCode(409, "expenseInvoiceItemId null");
            //if (await _repo.DeleteInvoiceItem(invoiceItemId) == null)
            //    return NotFound();
            #endregion
            await _repo.DeleteInvoiceItem(expenseInvoiceItemId);
            return Ok();
        }
        //Delete [baseUrl]/api/expense/deleteexpenseinvoice
        [HttpDelete]
        [Route("deleteexpenseinvoice")]
        public async Task<IActionResult> DeleteExpenseInvoice([FromHeader] int? companyId, [FromHeader] int? invoiceId)
        {
            //Check
            #region Check
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (invoiceId == null)
                return StatusCode(409, "expenseInvoiceId null");
            //if (await _repo.DeleteInvoice(companyId, invoiceId) == null)
            //    return StatusCode(409, "object null");
            #endregion
            await _repo.DeleteInvoice(companyId, invoiceId);
            return Ok();
        }
        #endregion

        //Expense
        #region Expense
        //Get [baseUrl]/api/expense/getexpenseinvoicebycontragentid
        [HttpGet]
        [Route("getexpenseinvoicebycontragentid")]
        public IActionResult GetExpenseInvoiceByContragentId([FromHeader]int? contragentId, [FromHeader] int? companyId)
        {
            if (contragentId == null)
                return StatusCode(409, "contragentId null");

            var invoice = _repo.GetExpenseInvoiceByContragentId(contragentId, companyId);
            if (invoice == null)
                return StatusCode(409, "invoice null");

            var invoiceToReturn = _mapper.Map<IEnumerable<ExpenseExInvoiceGetDto>>(invoice);
            return Ok(invoice);
        }
        //Get [baseUrl]/api/expense/getexpense
        [HttpGet]
        [Route("getexpense")]
        public async Task<IActionResult> GetExpense([FromQuery]PaginationParam productParam, [FromHeader]int? companyId)
        {
            //Checking
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckExpense(currentUserId, companyId))
                return Unauthorized();
            #endregion
            //Repo Get
            var invoices = await _repo.GetExpense(productParam, companyId);
            //Mapped object
            var invoiceToReturn = _mapper.Map<IEnumerable<ExpenseGetDto>>(invoices);

            return Ok(invoices);
        }
        //Post [baseUrl]/api/expense/createexpense
        [HttpPost]
        [Route("createexpense")]
        public async Task<IActionResult> CreateExpense([FromHeader] int? companyId, [FromHeader] int? contragentId, [FromBody]VwCreateExpense createExpense)
        {
            //mapped expense
            var mappedExpese = _mapper.Map<Expense>(createExpense.ExpensePostDto);
            //mapped incomeitems
            var mappedExpenseItem = _mapper.Map<List<ExpenseItem>>(createExpense.ExpenseItemPostDtos);

            //Check:
            #region Check
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (contragentId == null)
                return StatusCode(409, "contragentId null");
            if (createExpense.ExpensePostDto == null)
                return StatusCode(409, "ExpensePostDto null");
            if (createExpense.ExpenseItemPostDtos == null)
                return StatusCode(409, "ExpenseItemPostDtos null");
            //checking incomeitems paidmoney big than invoice total price  
            if (await _repo.CheckExpenseEqualingInvoiceTotalPriceForCreate(mappedExpenseItem))
                return StatusCode(411, "paidmoney big than totalmoney or one invoiceId doesnt exist");
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckExpense(currentUserId, companyId))
                return Unauthorized();

            // id-sinin olmasini yoxlayiriq.
            if (await _repo.CheckIncomeContragentIdInvoiceId(contragentId, companyId))
                return StatusCode(409, "contragentId  doesnt exist");
            if (_repo.CheckExpenseNegativeValue(mappedExpese, mappedExpenseItem))
                return StatusCode(428, "negative value is detected");

            #endregion

            var FromRepoIncomes = await _repo.CreateExpense(companyId, contragentId, mappedExpese, mappedExpenseItem);

            return Ok(FromRepoIncomes);
        }
        // get edit income
        //Get [baseUrl]/api/expense/geteditexpense    
        [HttpGet]
        [Route("geteditexpense")]
        public async Task<IActionResult> GetEditExpense([FromHeader] int? companyId, [FromHeader] int? expenseInvoiceId)
        {
            if (expenseInvoiceId == null)
                return StatusCode(409, "expenseInvoiceId null");
            if (companyId == null)
                return StatusCode(409, "companyId null");
            //Repo Get
            //var incomeItemsinvoices = await _repo.GetEditAllIncomes(companyId,invoiceId);
            var expenseItemsinvoices = await _repo.GetExpenseExpenseItem(companyId, expenseInvoiceId);

            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckExpense(currentUserId, companyId))
                return Unauthorized();
            if (expenseItemsinvoices == null)
                return StatusCode(406, "object null");
            #endregion

            // Mapped object
            var ToReturn = _mapper.Map<ExpenseExInvoiceEditGetDto>(expenseItemsinvoices);

            return Ok(ToReturn);
        }
        //Put [baseUrl]/api/income/updateexpense
        [HttpPut]
        [Route("updateexpense")]
        public async Task<IActionResult> UpdateExpense(VwExpensePut incomePut,[FromHeader] int? expenseInvoiceId, [FromHeader] int? companyId,[FromHeader] int? expenseId)
        {
            //Get edit income
            //didnt use yet
            Expense fromRepo = await _repo.GetEditExpense(expenseId, companyId);
            //mapping income
            Expense Mapped = _mapper.Map(incomePut.IncomePutDto, fromRepo);
            //Get edit incomeitems
            List<ExpenseItem> expenseItemsRepo = await _repo.GetEditExpenseItems(expenseId);
            //mapping incomeitems
            List<ExpenseItem> expenseItemsMapped = _mapper.Map(incomePut.ExpenseItemGetDtos, expenseItemsRepo);
            //Check:
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            if (await _repo.CheckExpense(currentUserId, companyId))
                return Unauthorized();
            if (await _repo.CheckExpenseEqualingInvoiceTotalPriceForUpdate(incomePut.ExpenseItemGetDtos))
                return StatusCode(411, "paidmoney big than totalmoney or that expenseinvoice  doesn't exist");
          
            if (_repo.CheckExpenseUpdateNegativeValue(incomePut.ExpenseItemGetDtos))
                return StatusCode(428, "negative value is detected");

            #endregion

            //Account:
            var UpdateAccountDebit = _repo.UpdateExpenseAccountDebit(companyId, incomePut.ExpenseItemGetDtos);
            var UpdateAccountKredit = _repo.UpdateExpenseAccountKredit(companyId, incomePut.ExpenseItemGetDtos);

            //Put expense and expenseitems
            var income = await _repo.EditExpense(incomePut.ExpenseItemGetDtos, expenseInvoiceId);

            return Ok();

        }
        //Delete [baseUrl]/api/income/deleteexpenseitem
        [HttpDelete]
        [Route("deleteexpenseitem")]
        public async Task<IActionResult> DeleteExpenseItem([FromHeader] int? expenseItemId)
        {
            if (expenseItemId == null)
                return StatusCode(409, "incomeItemId null");
            await _repo.DeleteExpenseItem(expenseItemId);

            return Ok();
        }
        #endregion
    }

}