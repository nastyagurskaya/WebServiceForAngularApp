using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServiceForAngular.BLL.Interfaces;
using WebServiceForAngular.DAL.Models;
using WebServiceForAngular.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebServiceForAngular.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ApiUser")]
    [ApiController]
    public class CheckPostsController : Controller
    {
        private readonly ICheckListPostService _checkPostService;
        private readonly ICheckItemService _checkItemService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IClaimPrincipalService _claimPrincipalService;

        public CheckPostsController(ICheckListPostService checkPostService, ICheckItemService checkItemService, IUserService userService, IMapper mapper, IClaimPrincipalService claimPrincipalService)
        {
            _claimPrincipalService = claimPrincipalService;
            _checkPostService = checkPostService;
            _checkItemService = checkItemService;
            _userService = userService;
            _mapper = mapper;
        }
       
        [HttpGet]
        public async Task<List<CheckListPost>> GetAll()
        {
            var userId = _claimPrincipalService.GetClaimFromHttp();
            var user = await _userService.GetUserByClaimAsync(userId);
            return await _checkPostService.GetCheckPostsByUserAsync(user.Id);
        }
        [HttpGet("checkitems")]
        public IEnumerable<CheckItem> GetCheckItems()
        {
           // var userId = _caller.Claims.Single(c => c.Type == "id");
            //var user = await _userService.GetUserByClaimAsync(userId);
            return _checkItemService.GetCheckItems();
        }
        [HttpGet("checkitems/{id}")]
        public IEnumerable<CheckItem> GetCheckItemsbiPostId(int id)
        {
            return _checkItemService.GetCheckItemsByCheckPost(id);
        }
        // POST api/<controller>
        [HttpPost("create")]
        public async Task<int> Create([FromBody]CheckPostViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            var ch = _mapper.Map<CheckListPost>(model);
            var userId = _claimPrincipalService.GetClaimFromHttp();
            var user = await _userService.GetUserByClaimAsync(userId);
            //if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            //await _appDbContext.User.AddAsync(new User { IdentityId = userIdentity.Id, Email = userIdentity.Email, Name = userIdentity.FirstName + " " + userIdentity.LastName, Username = userIdentity.UserName, Phone = userIdentity.PhoneNumber });
            //await _appDbContext.SaveChangesAsync();
            var checkpost= new CheckListPost {Title = ch.Title, Color = ch.Color,User = user };
            int id = _checkPostService.InsertCheckPost(checkpost);
            return id;
        }
        [HttpPost("checklistitem/create")]
        public async Task<int> CreateCheckListItem([FromBody]CheckItemViewModel model)
        {
            
            var ch = _mapper.Map<CheckItem>(model);
            var userId = _claimPrincipalService.GetClaimFromHttp();
            var user = await _userService.GetUserByClaimAsync(userId);

            var checkitem = new CheckItem { Body = ch.Body, Checked = ch.Checked, CheckListPostId =ch.CheckListPostId };
            int id = _checkItemService.InsertCheckItem(checkitem);
            return id;
        }
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody]CheckPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ch = _mapper.Map<CheckListPost>(model);
            var userId = _claimPrincipalService.GetClaimFromHttp();
            var user = await _userService.GetUserByClaimAsync(userId);

            var checkpost = new CheckListPost { Title = ch.Title, Color = ch.Color, User = user, Id = ch.Id };
            _checkPostService.UpdateCheckPost(checkpost);
            return new OkObjectResult("Post updated");

        }
        [HttpGet("{id}", Name = "GetCheckPost")]
        public ActionResult<CheckListPost> GetById(int id)
        {
            var item = _checkPostService.GetCheckListPost(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {

                _checkPostService.DeleteCheckPost(id);
                return new OkObjectResult("Post deleted");
            }
            return BadRequest();
        }
        [HttpDelete("checklistitem/{id}")]
        public IActionResult DeleteCheckItem(int id)
        {
            if (id != 0)
            {

                _checkItemService.DeleteCheckItem(id);
                return new OkObjectResult("Item deleted");
            }
            return BadRequest();
        }
        [HttpPost("checklistitem/update")]
        public async Task<IActionResult> UpdateCheckItem([FromBody]CheckItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ch = _mapper.Map<CheckItem>(model);

            var checkpost = new CheckItem { Checked = ch.Checked, Body = ch.Body, Id = ch.Id, CheckListPostId = ch.CheckListPostId };
            _checkItemService.UpdateCheckItem(checkpost);
            return new OkObjectResult("Item updated");

        }
    }
}
