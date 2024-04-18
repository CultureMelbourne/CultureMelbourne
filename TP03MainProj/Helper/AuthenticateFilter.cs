using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TP03MainProj.Helper
{
    public class AuthenticateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionName = filterContext.ActionDescriptor.ActionName;
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if (!(controllerName == "Home" && (actionName == "WebGate" || actionName == "VerifyPassword")))
            {
                if (filterContext.HttpContext.Session["Authenticated"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary {
                    { "controller", "Home" },
                    { "action", "WebGate" }
                        });
                }
            }
        }

    }

}