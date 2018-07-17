using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceForAngular.DAL.Models;

namespace WebServiceForAngular.BLL.Interfaces
{
    public interface IUserPostService
    {
        List<Post> GetSharedPosts(int id);
        void SharePost(UserPost userPost);
    }
}
