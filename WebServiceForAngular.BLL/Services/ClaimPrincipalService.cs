using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebServiceForAngular.BLL.Interfaces;

namespace WebServiceForAngular.BLL.Services
{
    public class ClaimPrincipalService : IClaimPrincipalService
    {
        private readonly ClaimsPrincipal _caller;
        public ClaimPrincipalService(IHttpContextAccessor httpContextAccessor)
        {
            _caller = httpContextAccessor.HttpContext.User;
        }

        public Claim GetClaimFromHttp()
        {
            return _caller.Claims.Single(c => c.Type == "id");
        }
    }
}
