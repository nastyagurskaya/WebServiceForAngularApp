using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using WebServiceForAngular.BLL.Interfaces;
using WebServiceForAngular.DAL.Models;
using WebServiceForAngular.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebServiceForAngular.BLL.Services
{
    public class CheckListPostService : ICheckListPostService
    {
        private IRepository<CheckListPost> checkPostRepository;

        public CheckListPostService(IRepository<CheckListPost> checkPostRepository)
        {
            this.checkPostRepository = checkPostRepository;
        }
        public CheckListPost GetCheckListPost(int id)
        {
            return checkPostRepository.Get(id);
        }

        public IEnumerable<CheckListPost> GetCheckPostsByUser(int id)
        {
            return checkPostRepository.Find(p => p.UserId == id);
        }

        public async Task<List<CheckListPost>> GetCheckPostsByUserAsync(int id)
        {
            var checkPosts = checkPostRepository.GetDbSet();
            var ps = from p in checkPosts
                     select p;
            ps = ps.Where(p => p.UserId == id);
            return await ps.ToListAsync();
        }

        public IEnumerable<CheckListPost> GetCheckListPosts()
        {
            return checkPostRepository.GetDbSet().Include(c => c.CheckList);
        }

        public void UpdateCheckPost(CheckListPost post)
        {
            checkPostRepository.Update(post);
        }

        public void DeleteCheckPost(int id)
        {
            CheckListPost post = checkPostRepository.Get(id);
            checkPostRepository.Remove(post);
            checkPostRepository.SaveChanges();
        }

        public int InsertCheckPost(CheckListPost post)
        {
            return checkPostRepository.Insert(post);
        }
        
    }
}
