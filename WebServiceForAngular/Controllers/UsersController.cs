using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServiceForAngular.BLL.Interfaces;
using WebServiceForAngular.DAL.Models;


namespace WebServiceForAngular.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ApiUser")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly IClaimPrincipalService _claimPrincipalService;

        public UsersController(IUserService userService, IPostService postService, IClaimPrincipalService claimPrincipalService)
        {
            
            _userService = userService;
            _postService = postService;
            _claimPrincipalService = claimPrincipalService;
        }
        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _userService.GetUsers();
        }
        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> GetById(int id)
        {
            var item = _userService.GetUser(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        [HttpGet("home")]
        public async Task<User> Home()
        {
            //var userId = _caller.Claims.Single(c => c.Type == "id");
            var userId = _claimPrincipalService.GetClaimFromHttp();
            var user = await _userService.GetUserByClaimAsync(userId);
            
            return user;
        }

    }

}