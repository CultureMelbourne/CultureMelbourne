using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public ActionResult DiverseCultures()
        {


            var dataList = new List<AgeDistribution>();
            string[] folders = { "China", "Japan", "Korea", "Philipines", "Vietnam" };

            foreach (var folder in folders)
            {
                var path = Server.MapPath($"~/DataHandle/{folder}/age_distrbution.json");
                var jsonData = System.IO.File.ReadAllText(path);
                var jsonArray = JArray.Parse(jsonData);

                foreach (var item in jsonArray)
                {
                    var censusData = new AgeDistribution
                    {
                        CensusId = (int)item["CensusId"],
                        CensusYear = (int)item["CensusYear"],
                        CountryName = (string)item["CountryName"],
                        AgeDistributions = new List<int>()
                    };

                    string[] ageKeys = { "0-4", "5-9", "10-14", "15-19", "20-24", "25-29", "30-34", "35-39", "40-44", "45-49", "50-54", "55-59", "60-64", "65-69", "70-74", "75-79", "80-84", "85-89", "90-94", "95-99", "100-104", "105-109", "110-114" };
                    foreach (var key in ageKeys)
                    {
                        censusData.AgeDistributions.Add((int)item[key]);
                    }

                    dataList.Add(censusData);
                }
            }

            return View(dataList);


        }





    }
}