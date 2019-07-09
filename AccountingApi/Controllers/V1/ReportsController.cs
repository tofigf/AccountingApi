using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountingApi.Data.Repository.Interface;
using AccountingApi.Helpers.Extentions;
using AccountingApi.Models.ProcudureDto;
using AccountingApi.Models.ProcudureDto.ParamsObject;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountingApi.Controllers.V1
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IAccountRepository _repo;
        private readonly IMapper _mapper;

        public ReportsController(IAccountRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // Mənfəət və Zərər
        //Get [baseUrl]/api/reports/inexeportquery
        [HttpGet]
        [Route("inexeportquery")]
        public async Task<IActionResult> InExReportQuery([FromQuery] ReportFilter reportFilter, [FromHeader] int? companyId)
        {
            var Inreport = await _repo.ReportQueryByCompanyId(reportFilter, companyId);

            var ExPensereport = await _repo.ReportExpenseQueryByCompanyId(reportFilter, companyId);

            var ToReturn = _mapper.MergeInto<InExReportDto>(Inreport, ExPensereport);

            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/reports/incomereportquery
        [HttpGet]
        [Route("incomereportquery")]
        public async Task<IActionResult> IncomeReportQuery([FromQuery] ReportFilter reportFilter, [FromHeader] int? companyId)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();
            #endregion

            var incomesFromQuery = await _repo.IncomesQueryByCompanyId(reportFilter, companyId);

            var ToReturn = _mapper.Map<List<IncomeReportDto>>(incomesFromQuery);


            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/reports/expensereportquery
        [HttpGet]
        [Route("expensereportquery")]
        public async Task<IActionResult> ExpenseReportQuery([FromQuery] ReportFilter reportFilter, [FromHeader] int? companyId)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();

            #endregion

            var expenseFromQuery = await _repo.ExpensesQueryByCompanyId(reportFilter, companyId);

            var ToReturn = _mapper.Map<List<ExpenseReportDto>>(expenseFromQuery);


            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/reports/productreportquery
        [HttpGet]
        [Route("productreportquery")]
        public async Task<IActionResult> ProductReportQuery([FromHeader] int? companyId, [FromHeader] int? DateUntil)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();

            #endregion

            var expenseFromQuery = await _repo.ProductsQueryByCompanyId(companyId, DateUntil);

            var ToReturn = _mapper.Map<List<ProductsReportDto>>(expenseFromQuery);


            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/reports/invoicereportquery
        [HttpGet]
        [Route("invoicereportquery")]
        public async Task<IActionResult> InvoiceReportQuery([FromHeader] int? companyId, [FromHeader] int? DateUntil)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();

            #endregion

            var invoiceFromQuery = await _repo.InvoiceReportQueryByCompanyId(companyId, DateUntil);

            var ToReturn = _mapper.Map<List<InvoiceReportDto>>(invoiceFromQuery);

            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/reports/expenseinvoicereportquery
        [HttpGet]
        [Route("expenseinvoicereportquery")]
        public async Task<IActionResult> ExpenseInvoiceReportQuery([FromHeader] int? companyId, [FromHeader] int? DateUntil)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();

            #endregion

            var invoiceFromQuery = await _repo.ExpenseInvoiceReportQueryByCompanyId(companyId, DateUntil);

            var ToReturn = _mapper.Map<List<ExpenseInvoiceReportDto>>(invoiceFromQuery);


            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/reports/contragentreportquery
        [HttpGet]
        [Route("contragentreportquery")]
        public async Task<IActionResult> ContragentReportQuery([FromHeader] int? companyId, [FromHeader] int? DateUntil)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();

            #endregion

            var invoiceFromQuery = await _repo.ContragentReportQueryByCompanyId(companyId, DateUntil);

            var ToReturn = _mapper.Map<List<ContragentFromQueryDto>>(invoiceFromQuery);


            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/reports/workerreportquery
        [HttpGet]
        [Route("workerreportquery")]
        public async Task<IActionResult> WorkerReportQuery([FromHeader] int? companyId, [FromHeader] int? DateUntil)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();

            #endregion

            var workerFromQuery = await _repo.WorkerReportQueryByCompanyId(companyId, DateUntil);

            var ToReturn = _mapper.Map<List<WorkerFromQueryDto>>(workerFromQuery);

            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/reports/netincomereportquery
        [HttpGet]
        [Route("netincomereportquery")]
        public async Task<IActionResult> NetIncomeReportQuery([FromHeader] int? companyId, [FromHeader] int? DateUntil)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();

            #endregion

            var netIncomeFromQuery = await _repo.NetIncomeReportQueryByCompanyId(companyId, DateUntil);

            var ToReturn = _mapper.Map<List<NetIncomeFromQueryDto>>(netIncomeFromQuery);

            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/reports/InvoiceReportByContragent
        [HttpGet]
        [Route("invoicereportbycontragent")]
        public async Task<IActionResult> InvoiceReportByContragent([FromHeader] int? companyId)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();

            #endregion

            var FromQuery = await _repo.InvoiceReportByContragents(companyId);

            var ToReturn = _mapper.Map<List<InvoiceReportByContragentDto>>(FromQuery);

            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/reports/ProductReportAll
        [HttpGet]
        [Route("productreportall")]
        public async Task<IActionResult> ProductReportAll([FromHeader] int? companyId, [FromQuery] ReportFilter reportFilter)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();

            #endregion

            var FromQuery = await _repo.ProductAll(companyId, reportFilter);

            var ToReturn = _mapper.Map<List<ProductAllDto>>(FromQuery);

            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/reports/expenseinvoicereportbycontragent
        [HttpGet]
        [Route("expenseinvoicereportbycontragent")]
        public async Task<IActionResult> ExpenseInvoiceReportByContragent([FromHeader] int? companyId)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();

            #endregion

            var FromQuery = await _repo.ExpenseInvoiceReportByContragents(companyId);

            var ToReturn = _mapper.Map<List<ExpenseInvoiceReportByContragentDto>>(FromQuery);

            return Ok(ToReturn);
        }
        //Get [baseUrl]/api/reports/ProductReportAllExpense
        [HttpGet]
        [Route("productreportallexpense")]
        public async Task<IActionResult> ProductReportAllExpense([FromHeader] int? companyId, [FromQuery] ReportFilter reportFilter)
        {
            //Check
            #region Check
            int? currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (companyId == null)
                return StatusCode(409, "companyId null");
            if (currentUserId == null)
                return Unauthorized();

            #endregion

            var FromQuery = await _repo.ProductAllExpense(companyId, reportFilter);

            var ToReturn = _mapper.Map<List<ProductExpenseAllDto>>(FromQuery);

            return Ok(ToReturn);
        }

    }
}