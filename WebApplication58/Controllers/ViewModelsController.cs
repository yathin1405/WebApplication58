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
    public class ViewModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ViewModels
        public ActionResult Index()
        {
            return View(db.ViewModels.ToList());
        }

        // GET: ViewModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewModel viewModel = db.ViewModels.Find(id);
            if (viewModel == null)
            {
                return HttpNotFound();
            }
            return View(viewModel);
        }

        // GET: ViewModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "view,Email,FirstName,LastName,checkInDate,checkOutDate,numGuests,TotalPrice")] ViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser useer = new ApplicationUser()
                {
                    Email = viewModel.Email,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName
                };
                Reservation rn = new Reservation()
                {
                    checkInDate = viewModel.checkInDate,
                    checkOutDate = viewModel.checkOutDate,
                    numGuests = viewModel.numGuests,
                };
                db.Users.Add(useer);
                db.reservations.Add(rn);
                db.ViewModels.Add(viewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: ViewModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewModel viewModel = db.ViewModels.Find(id);
            if (viewModel == null)
            {
                return HttpNotFound();
            }
            return View(viewModel);
        }

        // POST: ViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "view,Email,FirstName,LastName,checkInDate,checkOutDate,numGuests,TotalPrice")] ViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: ViewModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewModel viewModel = db.ViewModels.Find(id);
            if (viewModel == null)
            {
                return HttpNotFound();
            }
            return View(viewModel);
        }

        // POST: ViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewModel viewModel = db.ViewModels.Find(id);
            db.ViewModels.Remove(viewModel);
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
