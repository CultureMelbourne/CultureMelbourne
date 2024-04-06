using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TP03MainProj.Controllers
{
    public class EducationController : Controller
    {
        // GET: Education
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DiverseCultures()
        {
            return View();
        }

        public ActionResult MigrationStories()
        { 
            return View();

        }
}
}