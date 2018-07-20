using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebServiceForAngular.Auth;
using WebServiceForAngular.DAL.Models;
using WebServiceForAngular.Helpers;
using WebServiceForAngular.ViewModels;

namespace WebServiceForAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthController(UserManager<AppUser> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }
        [HttpPost]
        public async Task<IActionResult> Refresh(string token, string refreshToken)
        {
            //var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            //var username = principal.Identity.Name; //this is mapped to the Name claim by default

            //var user = _usersDb.Users.SingleOrDefault(u => u.Username == username);
            //if (user == null || user.RefreshToken != refreshToken) return BadRequest();

            //var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
            //var newRefreshToken = _tokenService.GenerateRefreshToken();

            //user.RefreshToken = newRefreshToken;
            //await _usersDb.SaveChangesAsync();

            //return new ObjectResult(new
            //{
            //    token = newJwtToken,
            //    refreshToken = newRefreshToken
            //});
            return null;
        }
        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}