using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceForAngular.DAL.Models;

namespace WebServiceForAngular.BLL.Interfaces
{
    public interface ICheckListPostService
    {
        CheckListPost GetCheckListPost(int id);
        List<CheckListPost> GetCheckListPosts();
        void UpdateCheckPost(CheckListPost post);
        void DeleteCheckPost(int id);
        int InsertCheckPost(CheckListPost post);
        List<CheckListPost> GetCheckPostsByUser(int id);
        Task<List<CheckListPost>> GetCheckPostsByUserAsync(int id);
    }
}
