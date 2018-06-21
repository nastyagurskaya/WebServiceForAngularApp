using System;
using System.Collections.Generic;
using System.Text;
using WebServiceForAngular.BLL.Interfaces;
using WebServiceForAngular.DAL.Models;
using WebServiceForAngular.DAL.Repositories;

namespace WebServiceForAngular.BLL.Services
{
    public class AppUserService : IAppUserService
    {
        private IAppUserRepository<AppUser> appUserRepository;
        public AppUserService(IAppUserRepository<AppUser> appUserRepository)
        {
            this.appUserRepository = appUserRepository;
        }
        public void DeleteAppUser(string id)
        {
            AppUser user = GetAppUser(id);
            appUserRepository.Remove(user);
            appUserRepository.SaveChanges();
        }

        public IEnumerable<AppUser> GetAppUsers()
        {
            return appUserRepository.GetAll();
        }

        public AppUser GetAppUser(string id)
        {
            return appUserRepository.Get(id);
        }

        public void InsertAppUser(AppUser user)
        {
            appUserRepository.Insert(user);
        }

        public void UpdateAppUser(AppUser user)
        {
            appUserRepository.Update(user);
        }
        public void InsertAppUserAsync(AppUser user)
        {
            appUserRepository.InsertAsync(user);
        }

       
    }
}
