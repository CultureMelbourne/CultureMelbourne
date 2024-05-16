using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using TP03MainProj.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Web.UI.WebControls;
using TP03MainProj.Helper;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace TP03MainProj.Controllers
{
    //[AuthenticateFilter]

    public class EventsController : Controller
    {
        private MyApplicationContext db = new MyApplicationContext();

        // GET: Events (Accepting culture for CalenderDate from Home Index view)
        public ActionResult Index(string culture)
        {
            var cultureName = System.IO.Path.GetFileNameWithoutExtension(culture);

            ViewBag.Culture = cultureName;
            return View();               
        }

   

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       


        // Modified method to map the events data to the calendar
        public JsonResult GetCulturalDates(string culture, DateTime start, DateTime end)
        {
            string filePath = Server.MapPath("~/Content/DataSource/melbourne_events.csv");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                BadDataFound = null,
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<EventMap>();  // Using the same mapping as GetEventsFromEventbrite
                var records = csv.GetRecords<Events>().ToList();

                var filteredDates = records
                    .Where(e => e.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase) &&
                                ((e.StartDate >= start && e.StartDate <= end) || (e.EndDate >= start && e.EndDate <= end)))
                    .Select(e => new
                    {
                        title = e.Title,
                        start = e.StartDate.ToString("yyyy-MM-dd"),
                        end = e.EndDate.ToString("yyyy-MM-dd")
                    }).ToList();

                return Json(filteredDates, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetEventsFromEventbrite(string culture, DateTime date)
        {
            string filePath = Server.MapPath("~/Content/DataSource/melbourne_events.csv");
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                BadDataFound = null,
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<EventMap>();
                var records = csv.GetRecords<Events>().ToList();

                var filteredEvents = records
                    .Where(e => e.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase) &&
                                date >= e.StartDate &&
                                date <= e.EndDate)
                    .ToList();

                ViewBag.FormattedDate = date.ToString("dd-MM-yyyy"); // Formatting date for display in the view
                return PartialView("_EventsList", filteredEvents);
            }
        }


        // For Mapping the CSV columns to the Events class properties
        public sealed class EventMap : ClassMap<Events>
        {
            public EventMap()
            {
                Map(m => m.Culture).Index(0);
                Map(m => m.Title).Index(1);
                Map(m => m.StartDate).Index(2).TypeConverterOption.Format("d/MM/yyyy");
                Map(m => m.EndDate).Index(3).TypeConverterOption.Format("d/MM/yyyy");
                Map(m => m.Description).Index(4);
                Map(m => m.Url).Index(5);
            }
        }

        // Function to load quiz data based on culture from JSON
        public ActionResult LoadQuizData(string culture)
        {
            string filePath = Server.MapPath($"~/Content/Quizzes/{culture}_cult_quiz.json");
            if (System.IO.File.Exists(filePath))
            {
                string jsonData = System.IO.File.ReadAllText(filePath);
                var quizData = JsonConvert.DeserializeObject<QuizData>(jsonData);
                var cultureData = quizData.Cultures.FirstOrDefault(c => c.Name.Equals(culture, StringComparison.OrdinalIgnoreCase));

                if (cultureData != null)
                {
                    // Create a new object that only includes the questions array
                    //var questionsOnly = new
                    //{
                    //    Questions = cultureData.Questions
                    //};
                    return Json(cultureData, JsonRequestBehavior.AllowGet);
                }
            }
            return HttpNotFound($"Quiz data not found for {culture}.");
        }

        [HttpGet]
        public ActionResult QuizPartial(string culture)
        {
            string filePath = Server.MapPath($"~/Content/Quizzes/{culture}_cult_quiz.json");
            if (System.IO.File.Exists(filePath))
            {
                string jsonData = System.IO.File.ReadAllText(filePath);
                var quizData = JsonConvert.DeserializeObject<QuizData>(jsonData);
                var cultureData = quizData.Cultures.FirstOrDefault(c => c.Name.Equals(culture, StringComparison.OrdinalIgnoreCase));

                if (cultureData != null)
                {
                    return PartialView("_CultureQuizPartial", cultureData);
                }
            }
            return HttpNotFound($"Quiz data not found for {culture}.");
        }




        //public ActionResult QuestionPartial(int questionId)
        //{
        //    var question = GetQuestionById(questionId); // Fetch the question based on ID or other criteria
        //    return PartialView("_QuestionPartial", question);
        //}

        //public Questions GetQuestionById(int questionNum)
        //{
        //    // Flattening the list of questions from all quizzes
        //    return _quizzes.SelectMany(quiz => quiz.Questions).FirstOrDefault(q => q.QuestionNum == questionNum);
        //}



        // Function to save high score to session
        public void SaveHighScore(int score)
        {
            var sessionScore = Session["HighScore"] as int? ?? 0;  // Handle possible null value and type casting
            if (score > sessionScore)
            {
                Session["HighScore"] = score;
            }
        }

        // Function to get current high score from session
        public ActionResult GetHighScore()
        {
            var score = Session["HighScore"] as int? ?? 0; // Handle null and ensure a default value of 0
            return Json(new { score = score }, JsonRequestBehavior.AllowGet);
        }

        // Reset high score (optional)
        public void ResetHighScore()
        {
            Session["HighScore"] = 0;
        }
    }
}
