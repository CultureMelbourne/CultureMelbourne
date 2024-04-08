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

namespace TP03MainProj.Controllers
{
    public class EventsController : Controller
    {
        private MyApplicationContext db = new MyApplicationContext();

        // GET: Events (Accepting culture for CalenderDate from Home Index view)
        public ActionResult Index(string culture)
        {
            ViewBag.Culture = culture;
            //var events = db.Events.Include(e => e.CalenderDate);
            return View();                //View(events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.CalenderDateId = new SelectList(db.CalenderDates, "CalenderDateId", "Title");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Culture,Title,Description,StartDate,EndDate,URL,CalenderDateId")] Events events)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(events);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CalenderDateId = new SelectList(db.CalenderDates, "CalenderDateId", "Title", events.CalenderDateId);
            return View(events);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            ViewBag.CalenderDateId = new SelectList(db.CalenderDates, "CalenderDateId", "Title", events.CalenderDateId);
            return View(events);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Culture,Title,Description,StartDate,EndDate,URL,CalenderDateId")] Events events)
        {
            if (ModelState.IsValid)
            {
                db.Entry(events).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CalenderDateId = new SelectList(db.CalenderDates, "CalenderDateId", "Title", events.CalenderDateId);
            return View(events);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Events events = db.Events.Find(id);
            db.Events.Remove(events);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public JsonResult GetCulturalDates(string culture, DateTime start, DateTime end)
        {
            string filePath = Server.MapPath("~/Content/DataSource/calendar_data.csv");
            
            var culturalDates = new List<CalenderDate>();
            // Read all lines from the CSV file
            var lines = System.IO.File.ReadAllLines(filePath);
            
            // Skip the header line if your CSV has one
            foreach (var line in lines.Skip(1))
            {
                var columns = line.Split(',');

                // Parse each column into the CalenderDate object, adjust indices as per your CSV structure
                var calenderDate = new CalenderDate
                {
                    Culture = columns[0],
                    Title = columns[1],
                    Start_Date = DateTime.Parse(columns[2]),
                    End_Date = DateTime.Parse(columns[3]),
                };

                culturalDates.Add(calenderDate);
            }

            // Filter and select as before
            var filteredDates = culturalDates
                .Where(d => d.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase)) //&& 
                            //((d.Start_Date >= start && d.Start_Date <= end) || 
                            //(d.End_Date >= start && d.End_Date <= end)))
                .Select(d => new
                {
                    title = d.Title,
                    start = d.Start_Date.ToString("yyyy-MM-dd"),
                    end = d.End_Date.ToString("yyyy-MM-dd"),
                }).ToList();

            return Json(filteredDates, JsonRequestBehavior.AllowGet);
        
             
            //var culturalDates = new List<CalenderDate>
            //{
            //    new CalenderDate { Culture = "Japanese", Title = "Tanabata", Start_Date = new DateTime(2024, 7, 7), End_Date = new DateTime(2024, 7, 7)},
            //    new CalenderDate { Culture = "Chinese", Title = "Mid-Autumn Festival", Start_Date = new DateTime(2024, 9, 15), End_Date = new DateTime(2024, 9, 15)},
            //};

            //var filteredDates = culturalDates
            //    .Where(d => d.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase))
            //    .Select(d => new
            //    {
            //        title = d.Title,
            //        start = d.Start_Date.ToString("yyyy-MM-dd"),
            //        end = d.End_Date.ToString("yyyy-MM-dd"),
            //    }).ToList();

            //return Json(filteredDates, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEventsFromEventbrite(string culture, DateTime date)
        {
            /* POTENTIAL CODE FOR INTERNAL FILE READ
            string filePath = Server.MapPath("~/Content/events.csv"); // Adjust 'events.csv' to your CSV file name
            var allEvents = new List<Events>();

            // Read all lines from the CSV file
            var lines = System.IO.File.ReadAllLines(filePath);

            // Assume the first line is a header so skip it
            foreach (var line in lines.Skip(1))
            {
                var columns = line.Split(',');

                // Parse each column into an Events object, adjust indices based on your CSV structure
                var eventItem = new Events
                {
                    Culture = columns[0],
                    Title = columns[1],
                    StartDate = columns[2],
                    EndDate = columns[3],
                    Description = columns[4],
                    Url = columns[5],
                };

                allEvents.Add(eventItem);
            }

            var filteredEvents = allEvents
                .Where(e => e.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase) && e.StartDate == date.ToString("yyyy-MM-dd"))
                .Select(e => new
                {
                    title = e.Title,
                    start = e.StartDate,
                    end = e.EndDate,
                    description = e.Description,
                    url = e.Url
                }).ToList();

            return Json(filteredEvents, JsonRequestBehavior.AllowGet);
            */

            var allEvents = new List<Events> {
                new Events { Culture = "Japanese", Title = "Tanabata Festival", StartDate = "2024-07-07", EndDate = "2024-07-07", Description = "Star Festival in Japan.", Url = "http://example.com/tanabata" },
                new Events { Culture = "Japanese", Title = "AfterParty @ Tanabata", StartDate = "2024-07-07", EndDate = "2024-07-08", Description = "After party of the festival with DJ and snacks. ;)", Url = "http://example.com/after-tanabata" },
                new Events { Culture = "Chinese", Title = "Mid-Autumn Festival", StartDate = "2024-09-15", EndDate = "2024-09-15", Description = "Mooncake Festival.", Url = "http://example.com/mid-autumn" }
            };

            var filteredEvents = allEvents
                .Where(e => e.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase) &&
                    date >= DateTime.ParseExact(e.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) && // Not taking culture bounds for DateTime Format
                    date <= DateTime.ParseExact(e.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture))
                .Select(e => new
                {
                    title = e.Title,
                    start = e.StartDate,
                    end = e.EndDate,
                    description = e.Description,
                    url = e.Url
                }).ToList();

            return Json(filteredEvents, JsonRequestBehavior.AllowGet);
        }




    }
}
