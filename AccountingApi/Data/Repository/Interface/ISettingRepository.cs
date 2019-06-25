using AccountingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository.Interface
{
    public interface ISettingRepository
    {
        Task<List<Tax>> GetTaxs(int? companyId);
        Task<Tax> GetEditTax(int? companyId, int? taxId);
        Task<Tax> CreateTax(Tax tax, int? companyId);
        Task<Tax> UpdateTax(Tax tax);

        Task<List<Product_Unit>> GetProduct_Units(int? companyId);
        //check
        Task<bool> CheckUnit(int? currentUserId, int? companyId);
        Task<bool> CheckTax(int? currentUserId, int? companyId);
        //delete
        Task<Tax> DeleteTax(int? companyId, int? taxId);
    }
}
