using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using TP03MainProj.DataHandle;
using TP03MainProj.Helper;
using static System.Net.WebRequestMethods;

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

        [HttpPost]
        public JsonResult GetCountryData(string countryName)
        {
            var basePath = Server.MapPath("~/DataHandle/");
            var agePath = Path.Combine(basePath, countryName, "age_distrbution.json");
            var occupationPath = Path.Combine(basePath, countryName, "occupation.json");
            var populationPath = Path.Combine(basePath, countryName, "population.json");
            var religionPath = Path.Combine(basePath, countryName, "religion.json");

            CountryData countryData = new CountryData();
            countryData.CountryName = countryName;

            var ageDataList = LoadAgeDistributions(agePath);
            countryData.ageDistributions = ageDataList;

            var occupationJson = System.IO.File.ReadAllText(occupationPath);
            var occupationDataList = JsonConvert.DeserializeObject<List<OccupationData>>(occupationJson);
            countryData.occupation.AddRange(occupationDataList);

            var populationJson = System.IO.File.ReadAllText(populationPath);
            var populationDataList = JsonConvert.DeserializeObject<List<Population>>(populationJson);
            countryData.populations.AddRange(populationDataList);

            // 如果国家名不是日本，才加载宗教数据
            if (countryName != "Japan" && System.IO.File.Exists(religionPath))
            {
                var religionJson = System.IO.File.ReadAllText(religionPath);
                var religionDataList = JsonConvert.DeserializeObject<List<Religion>>(religionJson);
                countryData.religions.AddRange(religionDataList);
            }

            return Json(countryData);
        }


        public ActionResult DiverseCultures()
        {

            //DiverseCulturesViewModel diverseCulturesViewModel = new DiverseCulturesViewModel();
            //var basePath = Server.MapPath("~/DataHandle/");
            //string[] folders = { "china", "japan", "korea", "philipines", "vietnam" };
            //foreach (var folder in folders)
            //{
            //    CountryData countryData = new CountryData();
            //    countryData.CountryName = folder;

            //    var agePath = Path.Combine(basePath, folder, "age_distrbution.json");
            //    var occupationPath = Path.Combine(basePath, folder, "occupation.json");
            //    var populationPath = Path.Combine(basePath, folder, "population.json");
            //    var religionPath = Path.Combine(basePath, folder, "religion.json");
                
                
            //    var ageDataList = LoadAgeDistributions(agePath);
            //    countryData.ageDistributions = ageDataList;


            //    var occupationJson = System.IO.File.ReadAllText(occupationPath);
            //    var occupationDataList = JsonConvert.DeserializeObject<List<OccupationData>>(occupationJson);
            //    countryData.occupation.AddRange(occupationDataList);


            //    var populationJson = System.IO.File.ReadAllText(populationPath);
            //    var populationDataList = JsonConvert.DeserializeObject<List<Population>>(populationJson);
            //    countryData.populations.AddRange(populationDataList);

            //    if (folder != "Japan")
            //    {
            //        var religionJson = System.IO.File.ReadAllText(religionPath);
            //        var religionDataList = JsonConvert.DeserializeObject<List<Religion>>(religionJson);
            //        countryData.religions.AddRange(religionDataList);
            //    }
            //    else
            //    {

            //    }
            //    diverseCulturesViewModel.countryDatas.Add(countryData);

            //}

            return View();
        }


        public List<AgeDistribution> LoadAgeDistributions(string agePath) {

            var dataList = new List<AgeDistribution>();
            var jsonData = System.IO.File.ReadAllText(agePath);
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

            return dataList;
        }


    }
}