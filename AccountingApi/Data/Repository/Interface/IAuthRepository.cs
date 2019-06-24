using AccountingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository.Interface
{
  public  interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string email, string password);
        Task<bool> CompanyCount(int userId);
        Task<IEnumerable<Company>> UserCompanies(int? userId);
        Task<Company> UserCompany(int? userId);
        Task<bool> CheckUsersMail(string email);
        Task<bool> CompanyCountForRegister(int userId);
    }
}
