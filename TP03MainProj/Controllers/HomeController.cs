using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TP03MainProj.Helper;

namespace TP03MainProj.Controllers
{
    [AuthenticateFilter]

    public class HomeController : Controller
    {
        public ActionResult WebGate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyPassword(string password)
        {
            const string correctPassword = "Gv!9rB@3wS*z";  // password
            if (password == correctPassword)
            {
                Session["Authenticated"] = true;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = "Wrong Password!!!";
                return RedirectToAction("WebGate", "Home");
            }
        }



        public ActionResult Index()
        {
            return View();
        }

    }
}