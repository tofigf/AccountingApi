﻿using AccountingApi.Data.Repository.Interface;
using AccountingApi.Models.ProcudureDto;
using AccountingApi.Models.ProcudureDto.ParamsObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
        {
            _context = context;
        }
        //Return İncome sum
        //Income Report
        public async Task<IncomeFromQueryDto> ReportQueryByCompanyId(ReportFilter reportFilter, int? companyId)
        {
            var incomeFromQuery = await _context.InExReportQuery
                .FromSql("exec GetIncome {0},{1},{2},{3}",
                companyId, reportFilter.IsPaid, reportFilter.StartDate, reportFilter.EndDate).FirstOrDefaultAsync();

            return incomeFromQuery;
        }
        //Return Expense sum
        //Expense Report
        public async Task<ExFromQueryDtoDto> ReportExpenseQueryByCompanyId(ReportFilter reportFilter, int? companyId)
        {
            var exFromQuery = await _context.ExFromQuery
                .FromSql("exec GetExpenseReport {0},{1},{2},{3}",
                companyId, reportFilter.IsPaid, reportFilter.StartDate, reportFilter.EndDate).FirstOrDefaultAsync();

            return exFromQuery;
        }
        //List Incomes Report
        public async Task<List<IncomeReportDto>> IncomesQueryByCompanyId(ReportFilter reportFilter, int? companyId)
        {
            var incomeReports = await _context.IncomesFromQuery
               .FromSql("exec GetIncomesReport  {0},{1},{2},{3}",
               companyId, reportFilter.IsPaid, reportFilter.StartDate, reportFilter.EndDate).ToListAsync();

            return incomeReports;
        }
        // List Expenses Report
        public async Task<List<ExpenseReportDto>> ExpensesQueryByCompanyId(ReportFilter reportFilter, int? companyId)
        {
            var expenseReports = await _context.ExpensesFromQuery
               .FromSql("exec GetExpensesReport  {0},{1},{2},{3}",
               companyId, reportFilter.IsPaid, reportFilter.StartDate, reportFilter.EndDate).ToListAsync();

            return expenseReports;
        }
        //List Products Limited 4 Report
        public async Task<List<ProductsReportDto>> ProductsQueryByCompanyId(int? companyId, int? DateUntil)
        {

            var productsReports = await _context.ProductsFromQuery
               .FromSql("exec GetProductsCount  {0},{1}",
               companyId, DateUntil).ToListAsync();
            List<ProductsReportDto> productsReportDtos = new List<ProductsReportDto>();

            for (int i = 0; i < productsReports.Count; i++)
            {
                ProductsReportDto products = new ProductsReportDto
                {
                    Name = productsReports[i].Name,
                    PercentOfTotal = productsReports[i].PercentOfTotal,
                    SumQty = productsReports[i].SumQty,
                };
                productsReportDtos.Add(products);
            }

            return productsReportDtos;
        }
        //Invoice Report By Paid Status
        public async Task<List<InvoiceReportDto>> InvoiceReportQueryByCompanyId(int? companyId, int? DateUntil)
        {
            var invoiceReports = await _context.InvoiceFromQuery
              .FromSql("exec InvoiceReport  {0},{1}",
              companyId, DateUntil).ToListAsync();

            List<InvoiceReportDto> invoiceReportDtos = new List<InvoiceReportDto>();

            for (int i = 0; i < invoiceReports.Count; i++)
            {

                InvoiceReportDto invoice = new InvoiceReportDto();

                switch (invoiceReports[i].IsPaid)
                {
                    case 1:
                        invoice.Status = "Gözləmədə";
                        break;
                    case 2:
                        invoice.Status = "Qismən";
                        break;
                    case 3:
                        invoice.Status = "Ödənilib";
                        break;
                    case 4:
                        invoice.Status = "Ödənilməyib";
                        break;
                    default:
                        invoice.Status = "yoxdu";
                        break;
                }
                invoice.Total = invoiceReports[i].Total;
                invoice.IsPaid = invoiceReports[i].IsPaid;

                invoiceReportDtos.Add(invoice);

            }

            return invoiceReportDtos;
        }
        //Expense Report By Paid Status
        public async Task<List<ExpenseInvoiceReportDto>> ExpenseInvoiceReportQueryByCompanyId(int? companyId, int? DateUntil)
        {
            var invoiceReports = await _context.ExpenseInvoiceFromQuery
              .FromSql("exec ExpenseInvoiceReport  {0},{1}",
              companyId, DateUntil).ToListAsync();

            List<ExpenseInvoiceReportDto> invoiceReportDtos = new List<ExpenseInvoiceReportDto>();

            for (int i = 0; i < invoiceReports.Count; i++)
            {

                ExpenseInvoiceReportDto invoice = new ExpenseInvoiceReportDto();

                switch (invoiceReports[i].IsPaid)
                {
                    case 1:
                        invoice.Status = "Gözləmədə";
                        break;
                    case 2:
                        invoice.Status = "Qismən";
                        break;
                    case 3:
                        invoice.Status = "Ödənilib";
                        break;
                    case 4:
                        invoice.Status = "Ödənilməyib";
                        break;
                    default:
                        invoice.Status = "yoxdu";
                        break;
                }
                invoice.Total = invoiceReports[i].Total;
                invoice.IsPaid = invoiceReports[i].IsPaid;

                invoiceReportDtos.Add(invoice);

            }

            return invoiceReportDtos;
        }
        //Contragent Report 
        public async Task<List<ContragentFromQueryDto>> ContragentReportQueryByCompanyId(int? companyId, int? DateUntil)
        {
            var contragentReports = await _context.ContragentFromQuery
              .FromSql("exec ContragentsReport {0},{1}",
              companyId, DateUntil).ToListAsync();
            //Globalization 
            var culture = new System.Globalization.CultureInfo("az");
            foreach (var data in contragentReports)
            {
                //Changing data monthnumber monnth name
                string monthName = culture.DateTimeFormat.GetMonthName(data.MonthNumber ?? 0);

                data.MonthName = monthName;
            }

            return contragentReports;
        }
        //Worker Report 
        public async Task<List<WorkerFromQueryDto>> WorkerReportQueryByCompanyId(int? companyId, int? DateUntil)
        {
            var workerReports = await _context.WorkerFromQuery
            .FromSql("exec WorkerReport {0},{1}",
            companyId, DateUntil).ToListAsync();
            //Globalization 
            var culture = new System.Globalization.CultureInfo("az");
            foreach (var data in workerReports)
            {
                //Changing data monthnumber monnth name
                string monthName = culture.DateTimeFormat.GetMonthName(data.MonthNumber ?? 0);

                data.MonthName = monthName;
            }

            return workerReports;
        }
        //NetIncome Report
        public async Task<List<NetIncomeFromQueryDto>> NetIncomeReportQueryByCompanyId(int? companyId, int? DateUntil)
        {
            var netReports = await _context.NetIncomeFromQuery
           .FromSql("exec GetNetIncomeReport {0},{1}",
           companyId, DateUntil).ToListAsync();

            return netReports;
        }
        //InvoiceReportByContragent
        public async Task<List<InvoiceReportByContragentDto>> InvoiceReportByContragents(int? companyId)
        {
            var Reports = await _context.invoiceReportByContragents
         .FromSql("exec InvoiceReportByContragent {0}",
         companyId).ToListAsync();

            return Reports;
        }
        //ProductAll
        public async Task<List<ProductAllDto>> ProductAll(int? companyId, ReportFilter reportFilter)
        {
            var Reports = await _context.ProductAllDtoQuery
         .FromSql("exec GetProductsAllCount {0}",
         companyId, reportFilter.StartDate, reportFilter.EndDate).ToListAsync();

            return Reports;
        }
        //ExpenseInvoiceReportByContragent
        public async Task<List<ExpenseInvoiceReportByContragentDto>> ExpenseInvoiceReportByContragents(int? companyId)
        {
            var Reports = await _context.ExpenseInvoiceReportByContragents
         .FromSql("exec ExpenseInvoiceReportByContragent {0}",
         companyId).ToListAsync();

            return Reports;
        }
        //ProductAllExpense
        public async Task<List<ProductExpenseAllDto>> ProductAllExpense(int? companyId, ReportFilter reportFilter)
        {
            var Reports = await _context.ProductExpenseAllQuery

         .FromSql("exec GetProductsAllCountByExpense {0},{1},{2}",
         companyId, reportFilter.StartDate, reportFilter.EndDate).ToListAsync();

            return Reports;
        }
        //GetInvoiceProductCountById
        public async Task<List<GetInvoiceProductCountByIdDto>> GetInvoiceProductCountById(int? companyId, int? productId)
        {
            var Reports = await _context.GetInvoiceProductCountByIdQuery

         .FromSql("exec GetInvoiceProductCountById {0},{1}",
         companyId, productId).ToListAsync();

            return Reports;
        }
        //InvoicesReportByContragentId
        public async Task<List<InvoicesReportByContragentIdDto>> InvoicesReportByContragentId(int? companyId, int? contragentId)
        {
            var Reports = await _context.InvoicesReportByContragentIdQuery

         .FromSql("exec InvoicesReportByContragentId {0},{1}",
         companyId, contragentId).ToListAsync();

            return Reports;
        }
        //GetExpenseInvoiceProductCountById
        public async Task<List<GetExpenseInvoiceProductCountByIdDto>> GetExpenseInvoiceProductCountById(int? companyId, int? productId)
        {
            var Reports = await _context.GetExpenseInvoiceProductCountByIdQuery

         .FromSql("exec GetExpenseInvoiceProductCountById {0},{1}",
         companyId, productId).ToListAsync();

            return Reports;
        }
        //ExpenseInvoiceReportByContragentId
        public async Task<List<ExpenseInvoiceReportByContragentIdDto>> ExpenseInvoiceReportByContragentId(int? companyId, int? contragentId)
        {
            var Reports = await _context.ExpenseInvoiceReportByContragentIdQuery

         .FromSql("exec ExpenseInvoiceReportByContragentId {0},{1}",
         companyId, contragentId).ToListAsync();

            return Reports;
        }

    }
}
