using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Humanizer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using System.IO;
using System;
using Microsoft.AspNetCore.Authorization;

namespace CabFrontend.Services
{
    public class AuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the user is not authenticated
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // Check if the request is marked with [AllowAnonymous]
                var allowAnonymous = context.ActionDescriptor.EndpointMetadata
                    .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

                if (!allowAnonymous && !context.HttpContext.Request.Path.Value.Contains("/User/LoginandSignup"))
                {
                    // User is not authenticated, redirect to the login page
                    context.Result = new RedirectToActionResult("LoginandSignup", "User", null);
                }
            }
        }
    }

}





