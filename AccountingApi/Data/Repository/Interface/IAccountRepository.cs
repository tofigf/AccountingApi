using AccountingApi.Models.ProcudureDto;
using AccountingApi.Models.ProcudureDto.ParamsObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository.Interface
{
    public interface IAccountRepository
    {
        Task<IncomeFromQueryDto> ReportQueryByCompanyId(ReportFilter reportFilter, int? companyId);
        Task<ExFromQueryDtoDto> ReportExpenseQueryByCompanyId(ReportFilter reportFilter, int? companyId);
        Task<List<IncomeReportDto>> IncomesQueryByCompanyId(ReportFilter reportFilter, int? companyId);
        Task<List<ExpenseReportDto>> ExpensesQueryByCompanyId(ReportFilter reportFilter, int? companyId);
        Task<List<ProductsReportDto>> ProductsQueryByCompanyId(int? companyId, int? DateUntil);
        Task<List<InvoiceReportDto>> InvoiceReportQueryByCompanyId(int? companyId, int? DateUntil);
        Task<List<ExpenseInvoiceReportDto>> ExpenseInvoiceReportQueryByCompanyId(int? companyId, int? DateUntil);
        Task<List<ContragentFromQueryDto>> ContragentReportQueryByCompanyId(int? companyId, int? DateUntil);
        Task<List<WorkerFromQueryDto>> WorkerReportQueryByCompanyId(int? companyId, int? DateUntil);
        Task<List<NetIncomeFromQueryDto>> NetIncomeReportQueryByCompanyId(int? companyId, int? DateUntil);
        Task<List<InvoiceReportByContragentDto>> InvoiceReportByContragents(int? companyId);
        Task<List<ProductAllDto>> ProductAll(int? companyId, ReportFilter reportFilter);
        Task<List<ExpenseInvoiceReportByContragentDto>> ExpenseInvoiceReportByContragents(int? companyId);
        Task<List<ProductExpenseAllDto>> ProductAllExpense(int? companyId, ReportFilter reportFilter);

    }
}
