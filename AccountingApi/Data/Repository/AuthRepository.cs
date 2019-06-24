using AccountingApi.Data.Repository.Interface;
using AccountingApi.Helpers;
using AccountingApi.Models;
using CryptoHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository
{
    public class AuthRepository: IAuthRepository
    {

        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CompanyCount(int userId)
        {
            var count = await _context.Companies.CountAsync(a => a.UserId == userId);
            if (count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<bool> CompanyCountForRegister(int userId)
        {
            int? count = await _context.Companies.CountAsync(a => a.UserId == userId);
            if (count == 0)
                return true;

            return false;
        }

        //Login:
        public async Task<User> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
                return null;
            if (!CryptoHelper.Crypto.VerifyHashedPassword(user.Password, password))
                return null;
            //IsPaid Status
            var userCompany = await _context.Companies.Where(w => w.UserId == user.Id).ToListAsync();
            if (user == null)
                return null;

            return user;
        }
        //List of  user companies
        public async Task<IEnumerable<Company>> UserCompanies(int? userId)
        {

            var userCompanies = await _context.Companies.Where(w => w.UserId == userId).ToListAsync();

            return userCompanies;
        }
        //When registering return created company
        public async Task<Company> UserCompany(int? userId)
        {

            var userCompany = await _context.Companies.SingleOrDefaultAsync(w => w.UserId == userId);

            return userCompany;
        }

        //Register
        public async Task<User> Register(User user, string password)
        {
            user.Password = CryptoHelper.Crypto.HashPassword(user.Password);
            user.Token = CryptoHelper.Crypto.HashPassword(DateTime.Now.ToLongDateString() + user.Email);
            user.Status = true;

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
            //email gondermek
            //SendUserInviting(user.Id);
            return user;
        }

        //Check:
        public async Task<bool> UserExists(string email, string password)
        {
            var loginned = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (!await _context.Users.AnyAsync(x => x.Email == email))
                return true;

            if (!Crypto.VerifyHashedPassword(loginned.Password, password))
                return true;

            return false;
        }
        //Check Already Exist User
        public async Task<bool> CheckUsersMail(string email)
        {
            if (await _context.Users.FirstOrDefaultAsync(u => u.Email == email) == null)
                return false;

            return true;
        }

    }
}
