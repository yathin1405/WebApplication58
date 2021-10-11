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
    public class CancellationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cancellations
        public ActionResult Index()
        {
            return View(db.Cancellations.ToList());
        }

        // GET: Cancellations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cancellation cancellation = db.Cancellations.Find(id);
            if (cancellation == null)
            {
                return HttpNotFound();
            }
            return View(cancellation);
        }

        // GET: Cancellations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cancellations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CancelID,checkInDate,checkOutDate,numGuests,Email,RoomNo,DateBooked,checkedIn,checkedOut,date_CheckedOut,DayNo,bookings,TotalPrice,Discount,Status")] Cancellation cancellation)
        {
            if (ModelState.IsValid)
            {
                db.Cancellations.Add(cancellation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cancellation);
        }

        // GET: Cancellations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cancellation cancellation = db.Cancellations.Find(id);
            if (cancellation == null)
            {
                return HttpNotFound();
            }
            return View(cancellation);
        }

        // POST: Cancellations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CancelID,checkInDate,checkOutDate,numGuests,Email,RoomNo,DateBooked,checkedIn,checkedOut,date_CheckedOut,DayNo,bookings,TotalPrice,Discount,Status")] Cancellation cancellation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cancellation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cancellation);
        }

        // GET: Cancellations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cancellation cancellation = db.Cancellations.Find(id);
            if (cancellation == null)
            {
                return HttpNotFound();
            }
            return View(cancellation);
        }

        // POST: Cancellations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cancellation cancellation = db.Cancellations.Find(id);
            db.Cancellations.Remove(cancellation);
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
