using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication58.Models;
using Microsoft.AspNet.Identity;


namespace WebApplication58.Controllers
{
    [Authorize]
    public class BookEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookEvents
        public ActionResult Index()
        {
            var bookEvent = db.bookEvent.Include(b => b.addevents);
            return View(bookEvent.ToList());
        }
        public ActionResult Index1()
        {
            var userid = User.Identity.GetUserId();
            var reserves = db.bookEvent.Where(r => r.Id == userid).Include(r => r.applicationUser);

            var reservations = db.bookEvent.Include(r => r.applicationUser);
            return View(reserves.ToList());
        }

        // GET: BookEvents/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookEvent bookEvent = db.bookEvent.Find(id);
            if (bookEvent == null)
            {
                return HttpNotFound();
            }
            return View(bookEvent);
        }

        // GET: BookEvents/Create
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(db.addEventD, "EventId", "EventType");
            return View();
        }

        // POST: BookEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,GName,NumAtt,date,Dduration,Nduration,EventId,DayDiscount,DayBasicCost,NightDiscount,NightBasicCost,TotalAmount")] BookEvent bookEvent)
        {
            var userid = User.Identity.GetUserId();
            bookEvent.Id = userid;
            var email = User.Identity.Name;
            bookEvent.Email = email;

            if (ModelState.IsValid)
            {


                bookEvent.CheckoutDate = bookEvent.CheckoutD();
                bookEvent.DayBasicCost = bookEvent.CalcDayBasicCost();
                bookEvent.DayDiscount = bookEvent.CalcDayDiscount();
                bookEvent.NightBasicCost = bookEvent.CalcNightBasicCost();
                bookEvent.NightDiscount = bookEvent.CalcNightDiscount();
                bookEvent.TotalAmount = bookEvent.CalcTotalAmountDue();
            }
            if (bookEvent.NumAtt > bookEvent.getmaxNUmAtt())
            {
                ModelState.AddModelError("", "Maximum number of attendies exceeded, it must be  <= " + bookEvent.getmaxNUmAtt());
            }

            if (bookEvent.dateChekeer(bookEvent.date) == true)
            {
                db.bookEvent.Add(bookEvent);
                db.SaveChanges();
                return RedirectToAction("Index1");
            }
            else
            {
                ModelState.AddModelError("", "The event is already booked please book for another one or book for a date later or equal to" + bookEvent.outDate());
            }

            ViewBag.EventId = new SelectList(db.addEventD, "EventId", "EventType", bookEvent.EventId);
            return View(bookEvent);
        }

        // GET: BookEvents/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookEvent bookEvent = db.bookEvent.Find(id);
            if (bookEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventId = new SelectList(db.addEventD, "EventId", "EventType", bookEvent.EventId);
            return View(bookEvent);
        }

        // POST: BookEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookEvent bookevent = db.bookEvent.Find(id);
            if (bookevent == null)
            {
                return HttpNotFound();
            }
            return View(bookevent);
        }

        // GET: BookEvents/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookEvent bookEvent = db.bookEvent.Find(id);
            if (bookEvent == null)
            {
                return HttpNotFound();
            }
            return View(bookEvent);
        }

        // POST: BookEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BookEvent bookEvent = db.bookEvent.Find(id);
            db.bookEvent.Remove(bookEvent);
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
