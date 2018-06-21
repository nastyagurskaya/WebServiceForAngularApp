using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebServiceForAngular.DAL.Models;

namespace WebServiceForAngular.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUser(int id);
        void InsertUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        Task<User> GetUserByClaimAsync(Claim claimId);
        //User GetUserByClaim(Claim claimId);
        //void InsertUserAsync(User user);
    }
}
