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
    public class UserPostService : IUserPostService
    {
        private IRepository<Post> postRepository;
        private IRepository<UserPost> userPostRepository;

        public UserPostService(IRepository<UserPost> userPostRepository, IRepository<Post> postRepository)
        {
            this.userPostRepository = userPostRepository;
            this.postRepository = postRepository;
        }

       

        public List<Post> GetSharedPosts(int id)
        {
            var userposts = userPostRepository.Find(p => p.UserId == id).Select(x => x.PostId);
            //var userposts = userPostRepository.GetDbSet()
            //var posts = new List<Post>();
           // foreach (UserPost up in userposts)
           // {
            var posts = postRepository.GetDbSet().Include(p => p.User).Where(p => userposts.Contains(p.Id));
               // posts.Add(post);
           // }
            return posts.ToList();
        }

        public void SharePost(UserPost userPost)
        {
            
            if (!userPostRepository.GetDbSet().Any(p => p.UserId==userPost.UserId && p.PostId == userPost.PostId))
            {
                userPostRepository.Insert(userPost);
            }
            
        }
    }
}
