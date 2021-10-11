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
    public class ShuttlesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Shuttles
        public ActionResult Index()
        {

            return View();
        }

        // GET: Shuttles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shuttle shuttle = db.shuttle.Find(id);
            if (shuttle == null)
            {
                return HttpNotFound();
            }
            return View(shuttle);
        }

        // GET: Shuttles/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Users, "Id", "Address");
            ViewBag.bookingId = new SelectList(db.reservations, "BookingID", "Id");
            return View();
        }

        // POST: Shuttles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PickupId,bookingId,Id,source,finacharge,pickDate,pickedup")] Shuttle shuttle)
        {
            if (ModelState.IsValid)
            {
                db.shuttle.Add(shuttle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Users, "Id", "Address", shuttle.Id);

            return View(shuttle);
        }

        // GET: Shuttles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shuttle shuttle = db.shuttle.Find(id);
            if (shuttle == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "Address", shuttle.Id);

            return View(shuttle);
        }

        // POST: Shuttles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PickupId,bookingId,Id,source,finacharge,pickDate,pickedup")] Shuttle shuttle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shuttle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "Address", shuttle.Id);

            return View(shuttle);
        }

        // GET: Shuttles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shuttle shuttle = db.shuttle.Find(id);
            if (shuttle == null)
            {
                return HttpNotFound();
            }
            return View(shuttle);
        }

        // POST: Shuttles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shuttle shuttle = db.shuttle.Find(id);
            db.shuttle.Remove(shuttle);
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
