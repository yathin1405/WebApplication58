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
    public class RoomTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RoomTypes
        public ActionResult Index()
        {
            return View(db.roomTypes.ToList());
        }
        public ActionResult RoomTypes()
        {
            return View(db.roomTypes.ToList());
        }

        // GET: RoomTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomTypes roomTypes = db.roomTypes.Find(id);
            if (roomTypes == null)
            {
                return HttpNotFound();
            }
            return View(roomTypes);
        }

        // GET: RoomTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]


        public ActionResult Create([Bind(Include = "RoomTypeId,Type,Description,Qty,NumOfRooms,NoBooked,BasicCharge")] RoomTypes roomTypes)
        {
            var types = db.roomTypes.Where(x => x.Type == roomTypes.Type);
            if (types.Count() != 0)
            {
                ModelState.AddModelError("", "The rootype already exists");
            }
            Rooms rm = new Rooms();
            if (ModelState.IsValid)
            {
                Rooms rooms = new Rooms();
                //roomTypes.NoBooked = rooms.NoBooked;
                roomTypes.NumOfRooms = roomTypes.Qty;
                //roomTypes.Qty = Convert.ToInt32(rm.ChangeQty());
                db.roomTypes.Add(roomTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roomTypes);
        }

        // GET: RoomTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomTypes roomTypes = db.roomTypes.Find(id);
            if (roomTypes == null)
            {
                return HttpNotFound();
            }
            return View(roomTypes);
        }

        // POST: RoomTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomTypeId,Type,Description,Qty,NumOfRooms,NoBooked,BasicCharge")] RoomTypes roomTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomTypes);
        }

        // GET: RoomTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomTypes roomTypes = db.roomTypes.Find(id);
            if (roomTypes == null)
            {
                return HttpNotFound();
            }
            return View(roomTypes);
        }

        // POST: RoomTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomTypes roomTypes = db.roomTypes.Find(id);
            db.roomTypes.Remove(roomTypes);
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
