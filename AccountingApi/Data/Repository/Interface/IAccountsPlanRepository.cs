using AccountingApi.Dtos.Account;
using AccountingApi.Dtos.AccountsPlan;
using AccountingApi.Models;
using AccountingApi.Models.ProcudureDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository.Interface
{
 public  interface IAccountsPlanRepository
    {
         Task<List<AccountsPlan>> ImportFromExcel(int? companyId);
        Task<List<AccountsPlan>> GetAccountsPlans(int? companyId);
        //From Procedure
        Task<List<BalanceSheetReturnDto>> BalanceSheet(int? companyId, DateTime? startDate, DateTime? endDate);

        //ManualJournal
        Task<List<OperationCategory>> GetOperationCategories();
        Task<ManualJournal> CreateManualJournal(int? companyId, ManualJournal manualJournal);
        Task<List<ManualJournal>> GetManualJournals(int? companyId);
        Task<ManualJournal> GetEditManualJournal(int? companyId, int? journalId);
        Task<ManualJournal> EditManualJournal(ManualJournal manualJournal);
        ManualJournalPostDto UpdateManualJournalAccountDebit(int? journalId, int? companyId, ManualJournalPostDto journalPostDto, int? OldDebitId);
        ManualJournalPostDto UpdateManualJournalAccountKredit(int? journalId, int? companyId, ManualJournalPostDto journalPostDto, int? OldKeditId);
        Task<ManualJournal> DeleteManualJournal(int? companyId, int? journalId);
        Task<List<JournalDto>> GetJournal(int? companyId, DateTime? startDate, DateTime? endDate);
    }
}
