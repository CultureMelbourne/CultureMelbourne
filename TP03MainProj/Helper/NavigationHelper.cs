using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TP03MainProj.Helper
{
    public static class NavigationHelper
    {
        public static string IsActive(ViewContext viewContext, string controller, string action, string parentController = null, string parentAction = null)
        {
            var currentController = viewContext.RouteData.Values["controller"].ToString();
            var currentAction = viewContext.RouteData.Values["action"].ToString();

            // Check if the current controller/action matches, or if a parent menu item should be active
            bool isActive = (controller == currentController && action == currentAction) ||
                            (parentController == currentController && parentAction == currentAction);

            return isActive ? "active" : "";
        }

    }
}