using DisasterRecovery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace DisasterRecovery.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var userId = Convert.ToString(httpContext.Session["LogedUserID"]);
            if (!string.IsNullOrEmpty(userId))
                {
                    var userRole = httpContext.Session["LogedUserRole"];
                    foreach (var role in allowedroles)
                    {
                        if (role == userRole.ToString()) return true;
                    }
                }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Users" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}