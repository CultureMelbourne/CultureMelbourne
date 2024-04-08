﻿using System;
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

namespace TP03MainProj.Controllers
{
    public class EventsController : Controller
    {
        private MyApplicationContext db = new MyApplicationContext();

        // GET: Events (Accepting culture for CalenderDate from Home Index view)
        public ActionResult Index(string culture)
        {
            ViewBag.Culture = culture;
            return View();               
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
                .Where(d => d.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase)) 
                .Select(d => new
                {
                    title = d.Title,
                    start = d.Start_Date.ToString("yyyy-MM-dd"),
                    end = d.End_Date.ToString("yyyy-MM-dd"),
                }).ToList();

            return Json(filteredDates, JsonRequestBehavior.AllowGet);

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
