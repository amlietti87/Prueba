using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace TECSO.FWK.ApiServices
{
    //public class Permissions : TypeFilterAttribute
    //{
    //    public Permissions(string claimValue) : base(typeof(ClaimRequirementFilter))
    //    {
    //        Arguments = new object[] { new Claim("Username", claimValue) };
    //    }
    //}

    //public class ClaimRequirementFilter : IAuthorizationFilter
    //{
    //    readonly Claim _claim;

    //    public ClaimRequirementFilter(Claim claim)
    //    {
    //        _claim = claim;
    //    }

    //    public void OnAuthorization(AuthorizationFilterContext context)
    //    {
    //        var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
    //        if (!hasClaim)
    //        {
    //            context.Result = new ForbidResult();
    //        }
    //    }
    //}
}
