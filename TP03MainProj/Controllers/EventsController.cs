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

        //Redundant method for Cultural Dates mapping
        /*
        public JsonResult GetCulturalDates(string culture, DateTime start, DateTime end)
        {
            string filePath = Server.MapPath("~/Content/DataSource/calendar_data.csv");

            var culturalDates = new List<CalenderDate>();
            // Read all lines from the CSV file
            var lines = System.IO.File.ReadAllLines(filePath);

            // Define an array of expected date formats
            var dateFormats = new[] { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "yyyy/MM/dd" };
            // Specify the CultureInfo; InvariantCulture is a good choice for a neutral culture
            var cultureInfo = CultureInfo.InvariantCulture;

            // Skip the header line if your CSV has one
            foreach (var line in lines.Skip(1))
            {
                var columns = line.Split(',');

                DateTime startDate, endDate;
                bool isStartValid = DateTime.TryParseExact(columns[2], dateFormats, cultureInfo, DateTimeStyles.None, out startDate);
                bool isEndValid = DateTime.TryParseExact(columns[3], dateFormats, cultureInfo, DateTimeStyles.None, out endDate);

                if (isStartValid && isEndValid)
                {
                    var calendarDate = new CalenderDate
                    {
                        Culture = columns[0],
                        Title = columns[1],
                        Start_Date = startDate,
                        End_Date = endDate,
                    };

                    culturalDates.Add(calendarDate);
                }
            }

            // Filter and select dates as before
            var filteredDates = culturalDates
                .Where(d => d.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase))
                .Select(d => new
                {
                    title = d.Title,
                    start = d.Start_Date.ToString("yyyy-MM-dd"),
                    end = d.End_Date.ToString("yyyy-MM-dd"),
                }).ToList();

            return Json(filteredDates, JsonRequestBehavior.AllowGet);
        }
        */

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


        // To retrieve data from API (using static DB for the time being)
        public JsonResult GetEventsFromEventbrite(string culture, DateTime date)
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
                    .Select(e => new
                    {
                        title = e.Title,
                        start = e.StartDate.ToString("yyyy-MM-dd"),
                        end = e.EndDate.ToString("yyyy-MM-dd"),
                        description = e.Description,
                        url = e.Url
                    }).ToList();

                return Json(filteredEvents, JsonRequestBehavior.AllowGet);
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
    }
}
