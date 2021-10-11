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
    public class AddPricesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AddPrices
        public ActionResult Index()
        {
            return View(db.AddPrices.ToList());
        }

        // GET: AddPrices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddPrice addPrice = db.AddPrices.Find(id);
            if (addPrice == null)
            {
                return HttpNotFound();
            }
            return View(addPrice);
        }

        // GET: AddPrices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AddPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "priceID,BPrice,LPrice,DPrice")] AddPrice addPrice)
        {
            if (ModelState.IsValid)
            {
                db.AddPrices.Add(addPrice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(addPrice);
        }

        // GET: AddPrices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddPrice addPrice = db.AddPrices.Find(id);
            if (addPrice == null)
            {
                return HttpNotFound();
            }
            return View(addPrice);
        }

        // POST: AddPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "priceID,BPrice,LPrice,DPrice")] AddPrice addPrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(addPrice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(addPrice);
        }

        // GET: AddPrices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddPrice addPrice = db.AddPrices.Find(id);
            if (addPrice == null)
            {
                return HttpNotFound();
            }
            return View(addPrice);
        }

        // POST: AddPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AddPrice addPrice = db.AddPrices.Find(id);
            db.AddPrices.Remove(addPrice);
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
