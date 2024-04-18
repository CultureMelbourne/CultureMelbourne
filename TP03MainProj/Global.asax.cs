using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace TP03MainProj
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            // Check for HTTP errors
            if (exception is HttpException httpException)
            {
                // Check if the error code is 404 - not found
                if (httpException.GetHttpCode() == 404)
                {
                    // Clear errors on the error server to prevent the default error page from being displayed
                    Server.ClearError();
                    // redirects to the Index action of the Home controller
                    Response.Redirect("~/Home/Index");
                }
            }
        }

    }
}
