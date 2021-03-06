﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServiceForAngular.DAL.Models;

namespace WebServiceForAngular.BLL.Interfaces
{
    public interface IPostService
    {
        List<Post> GetPosts();
        Post GetPost(int id);
        void UpdatePost(Post post);
        void DeletePost(int id);
        void InsertPost(Post post);
        List<Post> GetPostsByUser(int id);
        Task<List<Post>> GetPostsByUserAsync(int id);
    }
}
