using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebServiceForAngular.BLL.Interfaces;
using WebServiceForAngular.DAL.Models;
using WebServiceForAngular.ViewModels;

namespace WebServiceForAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(IUserService userService, UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            //if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            //await _appDbContext.User.AddAsync(new User { IdentityId = userIdentity.Id, Email = userIdentity.Email, Name = userIdentity.FirstName + " " + userIdentity.LastName, Username = userIdentity.UserName, Phone = userIdentity.PhoneNumber });
            //await _appDbContext.SaveChangesAsync();
            var user = new User { IdentityId = userIdentity.Id, Email = userIdentity.Email, Name = userIdentity.FirstName + " " + userIdentity.LastName, Username = userIdentity.UserName, Phone = userIdentity.PhoneNumber };
            _userService.InsertUser(user);
            return new OkObjectResult("Account created");
        }

    }
}