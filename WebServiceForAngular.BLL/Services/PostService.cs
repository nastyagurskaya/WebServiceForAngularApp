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
    public class PostService : IPostService
    {
        private IRepository<Post> postRepository;

        public PostService(IRepository<Post> postRepository)
        {
            this.postRepository = postRepository;
        }

        public void DeletePost(int id)
        {
            Post post = postRepository.Get(id);
            postRepository.Remove(post);
            postRepository.SaveChanges();
        }

        public Post GetPost(int id)
        {
            return postRepository.Get(id);
        }

        public IEnumerable<Post> GetPosts()
        {
            return postRepository.GetAll();
        }

        public IEnumerable<Post> GetPostsByUser(int id)
        {
            return postRepository.Find(p => p.UserId == id);
        }

        public async Task<List<Post>> GetPostsByUserAsync(int id)
        {
            var posts = postRepository.GetDbSet();
            var ps = from p in posts
                     select p;
            ps = ps.Where(p => p.UserId == id);
            return await ps.ToListAsync();
        }

        public void InsertPost(Post post)
        {
            postRepository.Insert(post);
        }

        public void UpdatePost(Post post)
        {
            postRepository.Update(post);
        }
    }
}
