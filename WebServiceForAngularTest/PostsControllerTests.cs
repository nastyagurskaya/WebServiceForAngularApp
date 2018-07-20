using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebServiceForAngular.BLL.Interfaces;
using WebServiceForAngular.Controllers;
using WebServiceForAngular.DAL.Models;
using WebServiceForAngular.DAL.Repositories;
using WebServiceForAngular.ViewModels;
using Xunit;

namespace WebServiceForAngularTest
{
    public class PostsControllerTests
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IUserPostService _userPostService;
        private readonly IClaimPrincipalService _claimService;
        private readonly IMapper _mapper;
        private readonly IRepository<Post> _repo;

        private readonly PostsController _controller;
        public PostsControllerTests()
        {
            _repo = Substitute.For<IRepository<Post>>();
            _postService = Substitute.For<IPostService>();
            _userService = Substitute.For<IUserService>();
            _userService.GetUserByClaimAsync(Arg.Any<Claim>()).Returns(Task<User>.FromResult(new User()));
            _userPostService = Substitute.For<IUserPostService>();
            _claimService = Substitute.For<IClaimPrincipalService>();
            _mapper = Substitute.For<IMapper>();
            _controller = Substitute.For<PostsController>(_postService, _userService, _userPostService, _mapper, _claimService);
        }
       
        [Fact(DisplayName = "PostsController delete post")]
        public void Posts_Delete()
        {
            _controller.Delete(1);
            _controller.Delete(0);
            _postService.Received().DeletePost(Arg.Is<int>(id => id > 0));
            _postService.DidNotReceive().DeletePost(0);
        }
        [Fact(DisplayName = "PostsController update post")]
        public async Task Posts_UpdateAsync()
        {
            _mapper.Map<Post>(Arg.Any<PostViewModel>()).Returns(new Post());
            await _controller.Update(Arg.Any<PostViewModel>());
            _claimService.Received().GetClaimFromHttp();
            await _userService.Received().GetUserByClaimAsync(Arg.Any<Claim>());
            _postService.Received().UpdatePost(Arg.Any<Post>());
        }
        [Fact(DisplayName = "PostController get all")]
        public async Task Posts_GetAllAsync()
        {
            await _controller.GetAll();
            _claimService.Received().GetClaimFromHttp();
            await _userService.Received().GetUserByClaimAsync(Arg.Any<Claim>());
            await _postService.Received().GetPostsByUserAsync(Arg.Any<int>());
        }
        [Fact(DisplayName = "PostController share post")]
        public async Task Posts_ShareAsync()
        {
            _mapper.Map<UserPost>(Arg.Any<UserPostViewModel>()).Returns(new UserPost());
            await _controller.SharePost(Arg.Any<UserPostViewModel>());
            _userPostService.Received().SharePost(Arg.Any<UserPost>());
        }
        [Fact(DisplayName = "PostsController create post")]
        public async Task Posts_CreateAsync()
        {
           _mapper.Map<Post>(Arg.Any<PostViewModel>()).Returns(new Post());
            await _controller.Create(Arg.Any<PostViewModel>());
            _claimService.Received().GetClaimFromHttp();
            await _userService.Received().GetUserByClaimAsync(Arg.Any<Claim>());
            _postService.Received().InsertPost(Arg.Any<Post>());
        }
        [Fact(DisplayName = "PostController get shared")]
        public async Task Posts_GetSharedAsync()
        {
            _userPostService.GetSharedPosts(Arg.Any<int>()).Returns(new List<Post>());
            await _controller.GetSharedPosts();
            _claimService.Received().GetClaimFromHttp();
            await _userService.Received().GetUserByClaimAsync(Arg.Any<Claim>());
            _userPostService.Received().GetSharedPosts(Arg.Any<int>());
        }
    }
}
