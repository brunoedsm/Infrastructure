using Domain.Security.Authentication.Api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Domain.Security.Authentication.Api.Helpers
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeFilterAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var resource = (Resource)context.HttpContext.Items["Resource"];
            if (resource == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
