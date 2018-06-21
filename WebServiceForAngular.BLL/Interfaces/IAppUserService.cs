using System;
using System.Collections.Generic;
using System.Text;
using WebServiceForAngular.DAL.Models;

namespace WebServiceForAngular.BLL.Interfaces
{
    public interface IAppUserService
    {
        IEnumerable<AppUser> GetAppUsers();
        AppUser GetAppUser(string id);
        void InsertAppUser(AppUser user);
        void UpdateAppUser(AppUser user);
        void DeleteAppUser(string id);
        void InsertAppUserAsync(AppUser user);
    }
}
