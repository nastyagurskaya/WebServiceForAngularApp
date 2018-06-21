using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceForAngular.BLL.Interfaces;
using WebServiceForAngular.DAL.Models;
using WebServiceForAngular.DAL.Repositories;

namespace WebServiceForAngular.BLL.Services
{
    public class CheckItemService : ICheckItemService
    {
        private IRepository<CheckItem> checkListRepository;

        public CheckItemService(IRepository<CheckItem> checkListRepository)
        {
            this.checkListRepository = checkListRepository;
        }

        public void DeleteCheckItem(int id)
        {
            CheckItem post = checkListRepository.Get(id);
            checkListRepository.Remove(post);
            checkListRepository.SaveChanges();
        }

        public async Task<List<CheckItem>> GetCheckItemsListByCheckPostAsync(int id)
        {
            var posts = checkListRepository.GetDbSet();
            var ps = from p in posts
                     select p;
            ps = ps.Where(p => p.CheckListPostId == id);
            return await posts.ToListAsync();
        }

        public CheckItem GetCheckItem(int id)
        {
            return checkListRepository.Get(id);
        }

        public IEnumerable<CheckItem> GetCheckItemsByCheckPost(int id)
        {
            return checkListRepository.Find(l => l.CheckListPostId == id);
        }

        public int InsertCheckItem(CheckItem post)
        {
            return checkListRepository.Insert(post);
        }

        public void UpdateCheckItem(CheckItem post)
        {
            checkListRepository.Update(post);
        }

        public IEnumerable<CheckItem> GetCheckItems()
        {
            return checkListRepository.GetDbSet();
        }
    }
}
