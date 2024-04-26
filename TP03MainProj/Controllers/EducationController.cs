using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TP03MainProj.DataHandle;
using TP03MainProj.Helper;

namespace TP03MainProj.Controllers
{
    //[AuthenticateFilter]

    public class EducationController : Controller
    {


        // GET: Education
        public ActionResult Index()
        {
            return View();
        }
        public List<AgeDistribution> DeserializeAgeData(string jsonData)
        {

            var Data = JsonConvert.DeserializeObject<List<AgeDistribution>>(jsonData);
            return Data;
        }
        public ActionResult DiverseCultures()
        {

          

            return View();


        }





    }
}