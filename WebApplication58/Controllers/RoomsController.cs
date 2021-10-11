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
    public class RoomsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rooms
        public ActionResult Index(string id)
        {
            var rooms = db.Rooms.Include(r => r.Roomtype);
            return View(rooms.ToList());
        }
        public ActionResult AllRooms()
        {
            var rooms = db.Rooms.Include(r => r.Roomtype);
            return View(rooms.ToList());
        }

        public ActionResult WalkIn()
        {
            var rooms = db.Rooms.Include(r => r.Roomtype);
            return View(rooms.ToList());
        }
        public ActionResult print()
        {
            var rooms = db.Rooms.Include(r => r.Roomtype);
            return View(rooms.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rooms rooms = db.Rooms.Find(id);
            if (rooms == null)
            {
                return HttpNotFound();
            }
            return View(rooms);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.RoomTypeId = new SelectList(db.roomTypes, "RoomTypeId", "Type");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomNo,RoomTypeId,FloorNum,Picture,RoomStatus,NoBooked,TotalPrice,NumOfRooms,DayNo,numguests,Numbeds")] Rooms rooms, HttpPostedFileBase img_upload)
        {
            byte[] data = null;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            rooms.Picture = data;

            RoomTypes rt = new RoomTypes();
            Reservation r = new Reservation();
            if (ModelState.IsValid)
            {
                rooms.RoomTypeId = rooms.RoomTypeId;
                rooms.RoomNo = rooms.generateRoomNumber();
                rooms.RoomStatus = "Available";
                rooms.NumOfRooms = rooms.numRoom();
                rooms.FloorNum = rooms.FloorNum;
                rooms.Roomtype = rooms.Roomtype;
                rooms.TotalPrice = rooms.price();
                db.Rooms.Add(rooms);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.RoomTypeId = new SelectList(db.roomTypes, "RoomTypeId", "Type", rooms.RoomTypeId);
            return View(rooms);
        }

        // GET: Rooms/Edit/5o
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rooms rooms = db.Rooms.Find(id);
            if (rooms == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomTypeId = new SelectList(db.roomTypes, "RoomTypeId", "Type", rooms.RoomTypeId);
            return View(rooms);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomNo,RoomTypeId,FloorNum,Picture,RoomStatus,NoBooked,TotalPrice,NumOfRooms,DayNo")] Rooms rooms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rooms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomTypeId = new SelectList(db.roomTypes, "RoomTypeId", "Type", rooms.RoomTypeId);
            return View(rooms);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rooms rooms = db.Rooms.Find(id);
            if (rooms == null)
            {
                return HttpNotFound();
            }
            return View(rooms);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Rooms rooms = db.Rooms.Find(id);
            db.Rooms.Remove(rooms);
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
