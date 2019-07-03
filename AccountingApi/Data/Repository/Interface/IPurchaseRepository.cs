using AccountingApi.Dtos.Purchase.Expense;
using AccountingApi.Dtos.Purchase.ExpenseInvoice;
using AccountingApi.Models;
using EOfficeAPI.Helpers.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository.Interface
{
    public interface IPurchaseRepository
    {
        //ExpenseInvoice
        #region ExpenseInvoice
        //Post
        Task<ExpenseInvoice> CreateInvoice(ExpenseInvoice invoice, int? companyId);
        Task<List<ExpenseInvoiceItem>> CreateInvoiceItems(List<ExpenseInvoiceItem> items, int? invoiceId);

        //Check
        #region Check
        Task<bool> CheckExpenseInvoice(int? currentUserId, int? companyId);
        Task<bool> CheckExpenseInvoiceProductId(List<ExpenseInvoiceItem> invoiceItems);
        Task<bool> CheckExpenseInvoiceId(int? invoiceId, int? companyId);
        bool CheckInvoiceNegativeValue(ExpenseInvoice invoice, List<ExpenseInvoiceItem> items);
        Task<bool> CheckExpenseInvoiceItem(int? invoiceId, List<ExpenseInvoiceItem> invoiceItems);
        #endregion

        //Get
        Task<List<ExpenseInvoiceGetDto>> GetInvoice(PaginationParam productParam, int? companyId);
        //get for edit invoice by id
        Task<ExpenseInvoice> GetEditInvoice(int? invoiceId, int? companyId);
        Task<List<ExpenseInvoiceItem>> GetEditInvoiceItem(int? invoiceId);
        Task<ExpenseInvoice> GetDetailInvoice(int? invoiceId, int? companyId);
        Task<Contragent> GetContragentInvoice(int? companyId, int? invoiceId);
        //Put
        // edit edit by id
        Task<ExpenseInvoice> EditInvoice(ExpenseInvoice invoice, List<ExpenseInvoiceItem> invoiceItems, int? invoiceId);
        Task<ExpenseInvoiceItem> DeleteInvoiceItem(int? invoiceItemId);
        Task<ExpenseInvoice> DeleteInvoice(int? companyId, int? invoiceId);
        //Accounting Update
        ExpenseInvoicePutDto UpdateInvoiceAccountDebit(int? invoiceId, int? companyId, ExpenseInvoicePutDto invoice, int? OldDebitId);
        ExpenseInvoicePutDto UpdateInvoiceAccountKredit(int? invoiceId, int? companyId, ExpenseInvoicePutDto invoice, int? OldKeditId);
        #endregion
        //Expense
        #region Expense
        //Get:
        List<ExpenseExInvoiceGetDto> GetExpenseInvoiceByContragentId(int? contragentId, int? companyId);
        Task<List<ExpenseGetDto>> GetExpense(PaginationParam productParam, int? companyId);
        Task<ExpenseInvoice> GetExpenseExpenseItem(int? companyId, int? invoiceId);
        Task<Expense> GetEditExpense(int? expenseId, int? companyId);
        Task<List<ExpenseItem>> GetEditExpenseItems(int? invoiceId);
        //Post:
        Task<Expense> CreateExpense(int? companyId, int? contragentId, Expense expense, List<ExpenseItem> expenseItems);
        //Put:
        Task<ExpenseItem> EditExpense(List<ExpenseItemGetDto> itemGetDtos, int? invoiceId);
        //Check:
        #region Check
        Task<bool> CheckExpense(int? currentUserId, int? companyId);
        Task<bool> CheckExpenseEqualingInvoiceTotalPriceForUpdate(List<ExpenseItemGetDto> expenseItems);
        Task<bool> CheckExpenseEqualingInvoiceTotalPriceForCreate(List<ExpenseItem> incomeItems);
        Task<bool> CheckIncomeContragentIdInvoiceId(int? contragentId, int? companyId);
        bool CheckExpenseNegativeValue(Expense expense, List<ExpenseItem> expenses);
        bool CheckExpenseUpdateNegativeValue(List<ExpenseItemGetDto> expenses);
        #endregion
        //Account:
        List<ExpenseItemGetDto> UpdateExpenseAccountDebit(int? companyId, List<ExpenseItemGetDto> incomeItem);
        List<ExpenseItemGetDto> UpdateExpenseAccountKredit(int? companyId, List<ExpenseItemGetDto> incomeItem);
        //Delete:
        Task<ExpenseItem> DeleteExpenseItem(int? expenseItemId);
        #endregion
    }
}
