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
    public class ToursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tours
        public ActionResult Index()
        {
            return View(db.Tours.ToList());
        }

        // GET: Tours/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        public ActionResult Detail()
        {
            string id = User.Identity.Name;
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.Email == id);
            var model = new RegisterViewModel(user);
            model.Tours.Sort();
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult guestDetails()
        {
            List<Tour> gd = new List<Tour>();
            var list = (from u in db.Users.Where(x => x.FirstName == HttpContext.User.Identity.Name)
                        join r in db.Tours on u.Email equals r.Email
                        //join rm in db.Rooms on r.RoomNo equals rm.RoomNo
                        //join rt in db.roomTypes on rm.RoomTypeId equals rt.RoomTypeId
                        select new
                        {
                            u.FirstName,
                            u.LastName,
                            u.Email,
                            u.PhoneNumber,
                            r.Tour_Date,
                            r.Tour_Duration,
                            r.Tour_Name,
                            r.Tour_Type,


                        }).ToList();
            foreach (var it in list)
            {
                Tour gdd = new Tour();
                gdd.Tour_Date = it.Tour_Date;
                gdd.Tour_Duration = it.Tour_Duration;
                gdd.Email = it.Email;
                gdd.Tour_Name = it.Tour_Name;
                gdd.Tour_Type = it.Tour_Type;
                //gdd.NumAChild = it.NumAChild;
                //gdd.NumAdults = it.NumAdults;
                //gdd.Airways = it.Airways;
                //gdd.Airline_Fee = it.Airline_Fee;
                //gdd.FROM = it.FROM;
                //gdd.TO = it.TO;
                //gdd.BasicCharge = it.BasicCharge;
                //gdd.TotalPrice = it.TotalPrice;
                //gdd.Discount = it.Discount;

                gd.Add(gdd);
            }
            return View(gd);
        }

        // GET: Tours/Create
        public ActionResult Create(string id)
        {
            var flight = db.Tours.Find(id);
            Session["TourId"] = id;
            ViewBag.Id = new SelectList(db.Users, "Id", "Email");
            ViewBag.tour = new SelectList(db.Tours, "Id", "FirstName");
            return View();
        }

        // POST: Tours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TourId,Tour_Type,Tour_Name,Tour_Duration,Tour_Date,Tour_Time")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Tours.Add(tour);
                db.SaveChanges();
                Session["bookID"] = tour.TourId;
                return RedirectToAction("ConfirmFlight", new { TourId = tour.TourId });
            }

            return View(tour);
        }

        public ActionResult ConfirmTour(int? TourId)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Flights.Add(flight);
            //    db.SaveChanges();
            //    Session["bookID"] = flight.FlightID;
            //    return RedirectToAction("Payment");
            //}
            {
                if (TourId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tour reservation = db.Tours.Find(TourId);
                if (reservation == null)
                {
                    return HttpNotFound();
                }
                //Flight flight = db.Flights.Where(x => x.FlightID == flight.Email).FirstOrDefault();

                //GuestDetails Gdetails = new GuestDetails()
                //{
                //    checkInDate = reservation.checkInDate,
                //    checkOutDate = reservation.checkOutDate,
                //    numGuests = reservation.numGuests,
                //    DayNo = reservation.DayNo,
                //    bookings = 0,
                //    RoomNo = reservation.RoomNo,
                //    FloorNum = rooms.FloorNum,
                //    Type = rooms.Roomtype.Type,
                //    BasicCharge = rooms.Roomtype.BasicCharge,
                //    TotalPrice = reservation.TotalPrice
                //};

                var rm = db.Tours.Find(reservation.TourId);
                //var rmb = db.roomTypes.Find(rm.RoomTypeId);


                Tour Gdetails = new Tour();
                Gdetails.Tour_Date = reservation.Tour_Date;
                Gdetails.Tour_Duration = reservation.Tour_Duration;
                Gdetails.Tour_Name = reservation.Tour_Name;
                Gdetails.Tour_Time = reservation.Tour_Time; ;
                Gdetails.Tour_Type = reservation.Tour_Type;

                Gdetails.Email = reservation.Email;


                //Gdetails.bookings = 0;
                //Gdetails.passenger_Cost = rm.passengerCost();
                //Gdetails.ReturnTicket_Price = rm.ReturnTicketPrice();
                //Gdetails.Seat_Type_Calc = rm.SeatTypeCalc();
                //Gdetails. = rm.BasicCharge;
                //Gdetails.TotalPrice = reservation.TotalPrice;
                //Gdetails.Discount = reservation.Discount;
                return View(Gdetails);
            }


        }
        //public ActionResult ConfirmFlight()
        //{
        //    return View("Payment");
        //}
        public ActionResult Payment()
        {
            return View(db.Tours.ToList());
        }

        // GET: Tours/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // POST: Tours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TourId,Tour_Type,Tour_Name,Tour_Duration,Tour_Date,Tour_Time")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tour);
        }

        // GET: Tours/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tour tour = db.Tours.Find(id);
            db.Tours.Remove(tour);
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
