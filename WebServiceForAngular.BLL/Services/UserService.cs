using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebServiceForAngular.BLL.Interfaces;
using WebServiceForAngular.DAL.Models;
using WebServiceForAngular.DAL.Repositories;

namespace WebServiceForAngular.BLL.Services
{
    public class UserService : IUserService
    {
        private IRepository<User> userRepository;
        private IRepository<Post> postRepository;

        public UserService(IRepository<User> userRepository, IRepository<Post> postRepository)
        {
            this.userRepository = userRepository;
            this.postRepository = postRepository;
        }

        public IEnumerable<User> GetUsers()
        {
            return userRepository.GetAll();
        }

        public User GetUser(int id)
        {
            return userRepository.Get(id);
        }

        public void InsertUser(User user)
        {
            userRepository.Insert(user);
        }
        public void UpdateUser(User user)
        {
            userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            Post post = postRepository.Get(id);
            postRepository.Remove(post);
            User user = GetUser(id);
            userRepository.Remove(user);
            userRepository.SaveChanges();
        }

        public async Task<User> GetUserByClaimAsync(Claim claimId)
        {
            return await userRepository.GetDbSet().Include(c => c.Identity).SingleAsync(c => c.Identity.Id == claimId.Value);
        }

        //public User GetUserByClaim(Claim claimId)
        //{
        //    User u = userRepository.GetDbSet().Include(c => c.Identity).(c => c.Identity.Id == claimId.Value);
        //    return u;
        //}

        //public void InsertUserAsync(User user)
        //{
        //    userRepository.InsertAsync(user);
        //}
    }
}
