using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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

        // Endpoint to get cultural dates
        public JsonResult GetCulturalDates(string culture, DateTime start, DateTime end)
        {
            // Retrieval or generation of cultural dates done here.
            // Below is an example with static data.
            var culturalDates = new List<CalenderDate>
            {    
                new CalenderDate { Culture = "Japanese", Title = "Tanabata", Start_Date = new DateTime(2024, 7, 7), End_Date = new DateTime(2024, 7, 7)},
                new CalenderDate { Culture = "Chinese", Title = "Mid-Autumn Festival", Start_Date = new DateTime(2024, 9, 15), End_Date = new DateTime(2024, 7, 7)},
                // Add more cultural dates as needed
            };

            // Filter the cultural dates based on the culture parameter and date range
            var filteredDates = culturalDates
                .Where(d => d.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase) && d.Start_Date >= start && d.Start_Date <= end)
                .Select(d => new {
                    title = d.Title,
                    start = d.Start_Date.ToString("yyyy-MM-dd"), // Format the date as a string for FullCalendar
                }).ToList();

            return Json(filteredDates, JsonRequestBehavior.AllowGet);
        }

        // Endpoint to get Eventbrite events
        public JsonResult GetEventsFromEventbrite(string culture, DateTime date)
        {
            // Call Eventbrite API or fetch data from the database here.
            // Below is an example with static data.
            var allEvents = new List<Events> {
                new Events { Culture = "Japanese", Title = "Tanabata Festival", StartDate = "2024-07-07", EndDate = "2024-07-07", Description = "Star Festival in Japan.", Url = "http://example.com/tanabata" },
                new Events { Culture = "Chinese", Title = "Mid-Autumn Festival", StartDate = "2024-09-15", EndDate = "2024-09-05", Description = "Mooncake Festival.", Url = "http://example.com/mid-autumn" }
                // Add more events with culture specified
            };

            var filteredEvents = allEvents.Where(e => e.Culture == culture && DateTime.Parse(e.StartDate) == date)
                                           .Select(e => new {
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
