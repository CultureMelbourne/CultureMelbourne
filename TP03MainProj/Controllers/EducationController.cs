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
        private AgeDistributionService _ageDistributionService = new AgeDistributionService();


        // GET: Education
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DiverseCultures()
        {

            var chinaDataJson = System.IO.File.ReadAllText(Server.MapPath("~/DataHandle/China/age_distrbution.json"));
            var japanDataJson = System.IO.File.ReadAllText(Server.MapPath("~/DataHandle/Japan/age_distrbution.json"));
            var koreaDataJson = System.IO.File.ReadAllText(Server.MapPath("~/DataHandle/Korea/age_distrbution.json"));
            var philippinesDataJson = System.IO.File.ReadAllText(Server.MapPath("~/DataHandle/Philipines/age_distrbution.json"));
            var vietnamDataJson = System.IO.File.ReadAllText(Server.MapPath("~/DataHandle/Vietnam/age_distrbution.json"));
            // Deserialise into a list of AgeDistribution objects.

            var chinaData = _ageDistributionService.DeserializeAgeData(chinaDataJson);
            var japanData = _ageDistributionService.DeserializeAgeData(japanDataJson);
            var koreaData = _ageDistributionService.DeserializeAgeData(koreaDataJson);
            var philippinesData = _ageDistributionService.DeserializeAgeData(philippinesDataJson);
            var vietnamData = _ageDistributionService.DeserializeAgeData(vietnamDataJson);

            // Pass the data to the view
            ViewBag.ChinaData = chinaData;
            ViewBag.JapanData = japanData;
            ViewBag.KoreaData = koreaData;
            ViewBag.PhilippinesData = philippinesData;
            ViewBag.VietnamData = vietnamData;

            return View();


        }





    }
}