using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TP03MainProj.Helper
{
    public static class NavigationHelper
    {
        public static string IsActive(ViewContext viewContext, string controller, string action)
        {
            string active = "";
            var routeData = viewContext.RouteData;

            string currentAction = (string)routeData.Values["action"];
            string currentController = (string)routeData.Values["controller"];

            if (controller == currentController && action == currentAction)
                active = "active";

            return active;
        }
    }
}