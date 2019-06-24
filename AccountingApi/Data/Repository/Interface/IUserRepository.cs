using AccountingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingApi.Data.Repository.Interface
{
   public interface IUserRepository
    {
        Task<Company> CreateCompany(Company companyCreate, int userId);

        Task<Company> GetEditCompany(int? companyId);

        Task<Company> EditCompany(Company companyEdit);

        Task<List<Company>> GetCompany(int? userId);

        Task<Company> GetCompanyById(int? userId, int? companyId);
        //Get EditUser
        Task<User> GetEditUser(int? userId);
        //Put User
        Task<User> EditUser(User user, string password);
        //Check:
        bool CheckOldPassword(string OldPassword, int? userId);
    }
}
