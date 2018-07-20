using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServiceForAngular.BLL.Interfaces;
using WebServiceForAngular.DAL.Models;
using WebServiceForAngular.Helpers;
using WebServiceForAngular.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebServiceForAngular.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ApiUser")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IUserPostService _userPostService;
        private readonly IMapper _mapper;
        private readonly IClaimPrincipalService _claimPrincipalService;

        public PostsController(IPostService postService, IUserService userService, IUserPostService userPostService, IMapper mapper, IClaimPrincipalService claimPrincipalService)
        {
            _claimPrincipalService = claimPrincipalService;
            _postService = postService;
            _userService = userService;
            _userPostService = userPostService;
            _mapper = mapper;
        }
        [HttpGet("{id}", Name = "GetPost")]
        public ActionResult<Post> GetById(int id)
        {
            var item = _postService.GetPost(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {

                _postService.DeletePost(id);
                return new OkObjectResult("Post deleted");
            }
            return BadRequest();
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]PostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var p = _mapper.Map<Post>(model);
            if (p == null)
            {
                return BadRequest(Errors.AddErrorToModelState("createpost_failure", "Error occupied while mapping", ModelState));
            }
            var userId = _claimPrincipalService.GetClaimFromHttp();
            var user = await _userService.GetUserByClaimAsync(userId);
            if (user == null)
            {
                return BadRequest(Errors.AddErrorToModelState("createpost_failure", "User did not found", ModelState));
            }
            var post = new Post { User = user, UserId = user.Id, Body = p.Body, Title = p.Title, Color = p.Color  };
            _postService.InsertPost(post);
            return new OkObjectResult("Post created");

        }
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody]PostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var p = _mapper.Map<Post>(model);
            var userId = _claimPrincipalService.GetClaimFromHttp();
            var user = await _userService.GetUserByClaimAsync(userId);
            if (user == null)
            {
                return BadRequest(Errors.AddErrorToModelState("updatepost_failure", "User did not found", ModelState));
            }
            var post = new Post { Id = p.Id, User = user, UserId = user.Id, Body = p.Body, Title = p.Title, Color = p.Color };
            _postService.UpdatePost(post);
            return new OkObjectResult("Post updated");

        }
        [HttpGet]
        public async Task<List<Post>> GetAll()
        {
            var userId = _claimPrincipalService.GetClaimFromHttp();
            var user = await _userService.GetUserByClaimAsync(userId);
            var posts = await _postService.GetPostsByUserAsync(user.Id);
            return posts;
        }
        [HttpGet("shared")]
        public async Task<List<Post>> GetSharedPosts()
        {
            var userId = _claimPrincipalService.GetClaimFromHttp();
            var user = await _userService.GetUserByClaimAsync(userId);
            var posts = _userPostService.GetSharedPosts(user.Id);
            return posts.ToList();
        }
        [HttpPost("shared/create")]
        public async Task<IActionResult> SharePost([FromBody]UserPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var p = _mapper.Map<UserPost>(model);
            var usPost = new UserPost { UserId = p.UserId, PostId = p.PostId };
            _userPostService.SharePost(usPost);
            return new OkObjectResult("Post shared");

        }
    }

}
