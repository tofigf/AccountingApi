using AccountingApi.Data.Repository.Interface;
using AccountingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository
{
    public class SettingRepository : ISettingRepository
    {
        private readonly DataContext _context;

        public SettingRepository(DataContext context)
        {
            _context = context;
        }
        //Get
        public async Task<List<Product_Unit>> GetProduct_Units(int? companyId)
        {
            if (companyId == null)
                return null;
            List<Product_Unit> units = await _context.Product_Units.Where(w => w.CompanyId == companyId).ToListAsync();

            return units;
        }
        public async Task<List<Tax>> GetTaxs(int? companyId)
        {
            if (companyId == null)
                return null;
            List<Tax> taxes = await _context.Taxes.Where(w => w.CompanyId == companyId).ToListAsync();

            return taxes;

        }
        public async Task<Tax> GetEditTax(int? companyId, int? taxId)
        {
            if (companyId == null)
                return null;
            if (taxId == null)
                return null;

            Tax tax = await _context.Taxes.FirstOrDefaultAsync(f => f.Id == taxId && f.CompanyId == companyId);
            if (tax == null)
                return null;
            return tax;
        }
        //Post
        public async Task<Tax> CreateTax(Tax tax, int? companyId)
        {
            if (companyId == null)
                return null;
            if (tax == null)
                return null;

            tax.CompanyId = Convert.ToInt32(companyId);

            await _context.Taxes.AddAsync(tax);

            await _context.SaveChangesAsync();

            return tax;
        }
        //Check
        public async Task<bool> CheckUnit(int? currentUserId, int? companyId)
        {
            if (currentUserId == null)
                return true;
            if (companyId == null)
                return true;
            if (await _context.Product_Units.AnyAsync(a => a.CompanyId == companyId && a.Company.UserId != currentUserId))
                return true;

            return false;
        }
        public async Task<bool> CheckTax(int? currentUserId, int? companyId)
        {
            if (currentUserId == null)
                return true;
            if (companyId == null)
                return true;
            if (await _context.Taxes.AnyAsync(a => a.CompanyId == companyId && a.Company.UserId != currentUserId))
                return true;

            return false;
        }
        //Put
        public async Task<Tax> UpdateTax(Tax tax)
        {
            if (tax == null)
                return null;

            _context.Entry(tax).State = EntityState.Modified;

            _context.Entry(tax).Property(a => a.CompanyId).IsModified = false;
            await _context.SaveChangesAsync();

            return tax;
        }

        public async Task<Tax> DeleteTax(int? companyId, int? taxId)
        {
            if (companyId == null)
                return null;
            Tax tax = await _context.Taxes.FirstOrDefaultAsync(f => f.CompanyId == companyId && f.Id == taxId);

            tax.IsDeleted = true;
            await _context.SaveChangesAsync();

            return tax;
        }

      
    }
}
