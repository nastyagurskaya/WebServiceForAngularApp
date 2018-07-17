using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceForAngular.DAL.Models;

namespace WebServiceForAngular.BLL.Interfaces
{
    public interface ICheckItemService
    {
        List<CheckItem> GetCheckItems();
        CheckItem GetCheckItem(int id);
        void UpdateCheckItem(CheckItem post);
        void DeleteCheckItem(int id);
        int InsertCheckItem(CheckItem post);
        List<CheckItem> GetCheckItemsByCheckPost(int id);
        Task<List<CheckItem>> GetCheckItemsListByCheckPostAsync(int id);
    }
}
