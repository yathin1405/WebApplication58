using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication58.Models;


namespace WebApplication58.Controllers
{
    [Authorize]
    public class AddEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AddEvents
        public ActionResult Index()
        {
            return View(db.addEventD.ToList());
        }

        // GET: AddEvents/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddEvents addEvents = db.addEventD.Find(id);
            if (addEvents == null)
            {
                return HttpNotFound();
            }
            return View(addEvents);
        }

        // GET: AddEvents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AddEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventId,EventType,MaxNumAttNum,DEventP,NEventP,DayLessD,DayGreaterD,NightLessD,NightGreaterD, MDQA")] AddEvents addEvents)
        {
            if (ModelState.IsValid)
            {
                db.addEventD.Add(addEvents);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(addEvents);
        }

        // GET: AddEvents/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddEvents addEvents = db.addEventD.Find(id);
            if (addEvents == null)
            {
                return HttpNotFound();
            }
            return View(addEvents);
        }

        // POST: AddEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventId,EventType,MaxNumAttNum,DEventP,NEventP,LessD,GreaterD")] AddEvents addEvents)
        {
            if (ModelState.IsValid)
            {
                db.Entry(addEvents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(addEvents);
        }

        // GET: AddEvents/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddEvents addEvents = db.addEventD.Find(id);
            if (addEvents == null)
            {
                return HttpNotFound();
            }
            return View(addEvents);
        }

        // POST: AddEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AddEvents addEvents = db.addEventD.Find(id);
            db.addEventD.Remove(addEvents);
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
    }
}
