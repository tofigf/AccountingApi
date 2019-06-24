using AccountingApi.Data.Repository.Interface;
using AccountingApi.Helpers;
using AccountingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        //Post:
        public async Task<Company> CreateCompany(Company companyCreate, int userId)
        {
            companyCreate.UserId = userId;
            companyCreate.CreatedAt = DateTime.Now;
            companyCreate.IsDeleted = false;
            //sekil yuklemek funksiyasi base 64
            companyCreate.PhotoFile = FileManager.Upload(companyCreate.PhotoFile);
            //seklini url-ni geri qaytarmaq ucun
            companyCreate.PhotoUrl = companyCreate.PhotoFile;

            await _context.Companies.AddAsync(companyCreate);

            await _context.SaveChangesAsync();

            Tax tax = new Tax
            {
                Name = "Vergisiz",
                IsDeleted = false,
                Rate = 0.0,
                CompanyId = companyCreate.Id
            };
            if (tax == null)
                return null;
            _context.Taxes.Add(tax);

            await _context.SaveChangesAsync();
            return companyCreate;
        }
        //Get:
        //get edit
        public async Task<Company> GetEditCompany(int? companyId)
        {
            if (companyId == null)
                return null;
            Company company = await _context.Companies.FindAsync(companyId);

            if (company == null)
                return null;
            return company;
        }
        public async Task<List<Company>> GetCompany(int? userId)
        {
            if (userId == null)
                return null;
            var company = await _context.Companies.Where(w => w.IsDeleted == false && w.UserId == userId).ToListAsync();

            return company;
        }
        public async Task<Company> GetCompanyById(int? userId, int? companyId)
        {
            if (userId == null)
                return null;
            if (companyId == null)
                return null;

            await _context.SaveChangesAsync();
            var company = await _context.Companies.FirstOrDefaultAsync(w => w.IsDeleted == false && w.UserId == userId && w.Id == companyId);
            if (company == null)
                return null;

            return company;
        }
        //Put:
        public async Task<Company> EditCompany(Company companyEdit)
        {
            if (companyEdit == null)
                return null;

            if (companyEdit.PhotoFile != null && companyEdit.PhotoFile != "")
            {
                //database-de yoxlayiriq eger bize gelen iscinin id-si ile eyni olan sekili silsin.
                string dbFileName = _context.Companies.FirstOrDefault(f => f.Id == companyEdit.Id).PhotoUrl;
                if (dbFileName != null)
                {
                    FileManager.Delete(dbFileName);
                }
                companyEdit.PhotoFile = FileManager.Upload(companyEdit.PhotoFile);
                //seklin url teyin edirik
                companyEdit.PhotoUrl = companyEdit.PhotoFile;
            }
            //update entitynin metodudu.
            _context.Entry(companyEdit).State = EntityState.Modified;
            _context.Entry(companyEdit).Property(a => a.UserId).IsModified = false;

            //Commit the transaction
            await _context.SaveChangesAsync();

            return companyEdit;

        }
        //Get Edit User
        public async Task<User> GetEditUser(int? userId)
        {
            if (userId == null)
                return null;

            User user = await _context.Users.FirstOrDefaultAsync(f => f.Id == userId);
            if (user == null)
                return null;

            return user;
        }
        //Put: User
        public async Task<User> EditUser(User user, string password)
        {

            User userForUpdate = _context.Users.Find(user.Id);

            userForUpdate.Password = CryptoHelper.Crypto.HashPassword(user.Password);
            //didnt use
            userForUpdate.Token = CryptoHelper.Crypto.HashPassword(DateTime.Now.ToLongDateString() + user.Email);
            userForUpdate.Status = true;

            await _context.SaveChangesAsync();

            return null;
        }
        //Check:
        public bool CheckOldPassword(string OldPassword, int? userId)
        {
            if (userId == null)
                return true;
            User user = _context.Users.Find(userId);

            if (!CryptoHelper.Crypto.VerifyHashedPassword(user.Password, OldPassword))
                return true;

            return false;
        }
    }
}
