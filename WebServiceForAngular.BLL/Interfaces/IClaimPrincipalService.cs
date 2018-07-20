using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace WebServiceForAngular.BLL.Interfaces
{
    public interface IClaimPrincipalService
    {
        Claim GetClaimFromHttp();
    }
}
