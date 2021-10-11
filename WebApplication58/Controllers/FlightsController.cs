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
    public class FlightsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Flights
        public ActionResult Index()
        {
            return View(db.Flights.ToList());
        }

        // GET: Flights/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        public ActionResult Detail()
        {
            string id = User.Identity.Name;
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.Email == id);
            var model = new RegisterViewModel(user);
            model.Flights.Sort();
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult guestDetails()
        {
            List<Flight> gd = new List<Flight>();
            var list = (from u in db.Users.Where(x => x.FirstName == HttpContext.User.Identity.Name)
                        join r in db.Flights on u.Email equals r.Email
                        //join rm in db.Rooms on r.RoomNo equals rm.RoomNo
                        //join rt in db.roomTypes on rm.RoomTypeId equals rt.RoomTypeId
                        select new
                        {
                            u.FirstName,
                            u.LastName,
                            u.Email,
                            u.PhoneNumber,
                            r.FROM,
                            r.TO,
                            r.NumAChild,
                            r.NumAdults,
                            r.Return_Date,
                            r.DepartureTime,
                            r.Departure_Date,
                            r.Airline_Fee,
                            r.Airways,

                        }).ToList();
            foreach (var it in list)
            {
                Flight gdd = new Flight();
                gdd.FirstName = it.FirstName;
                gdd.LastName = it.LastName;
                gdd.Email = it.Email;
                gdd.Departure_Date = it.Departure_Date;
                gdd.DepartureTime = it.DepartureTime;
                gdd.NumAChild = it.NumAChild;
                gdd.NumAdults = it.NumAdults;
                gdd.Airways = it.Airways;
                gdd.Airline_Fee = it.Airline_Fee;
                gdd.FROM = it.FROM;
                gdd.TO = it.TO;
                //gdd.BasicCharge = it.BasicCharge;
                //gdd.TotalPrice = it.TotalPrice;
                //gdd.Discount = it.Discount;

                gd.Add(gdd);
            }
            return View(gd);
        }

        // GET: Flights/Create
        public ActionResult Create(string id)
        {
            var flight = db.Flights.Find(id);
            Session["FlightID"] = id;
            ViewBag.Id = new SelectList(db.Users, "Id", "Email");
            ViewBag.flight = new SelectList(db.Flights, "Id", "FirstName");
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FlightID,Airways,FROM,TO,SeatType,FirstName,LastName,Email,Departure_Date,DepartureTime,Return_Flight,Return_Date,Return_Time,NumAdults,NumAChild,Seat_Type_Calc,Airline_Fee,ReturnTicket_Price,passenger_Cost,Add_Flight_Delay,Available_Seats,SEAT")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                db.Flights.Add(flight);
                db.SaveChanges();
                Session["bookID"] = flight.FlightID;
                return RedirectToAction("ConfirmFlight", new { FlightID = flight.FlightID });
            }

            return View(flight);
        }

        public ActionResult ConfirmFlight(int? FlightID)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Flights.Add(flight);
            //    db.SaveChanges();
            //    Session["bookID"] = flight.FlightID;
            //    return RedirectToAction("Payment");
            //}
            {
                if (FlightID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Flight reservation = db.Flights.Find(FlightID);
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

                var rm = db.Flights.Find(reservation.FlightID);
                //var rmb = db.roomTypes.Find(rm.RoomTypeId);


                Flight Gdetails = new Flight();
                Gdetails.TO = reservation.TO;
                Gdetails.FROM = reservation.FROM;
                Gdetails.Airways = reservation.Airways;
                Gdetails.Return_Date = reservation.Return_Date; ;
                Gdetails.Departure_Date = reservation.Departure_Date;
                Gdetails.DepartureTime = reservation.DepartureTime;
                Gdetails.NumAChild = reservation.NumAChild;
                Gdetails.NumAdults = reservation.NumAdults;
                Gdetails.FirstName = reservation.FirstName;
                Gdetails.LastName = reservation.LastName;
                Gdetails.Email = reservation.Email;


                //Gdetails.bookings = 0;
                Gdetails.passenger_Cost = rm.passengerCost();
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
            return View(db.Flights.ToList());
        }
        // GET: Flights/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FlightID,Airways,FROM,TO,SeatType,FirstName,LastName,Email,Departure_Date,DepartureTime,Return_Flight,Return_Date,Return_Time,NumAdults,NumAChild,Seat_Type_Calc,Airline_Fee,ReturnTicket_Price,passenger_Cost,Add_Flight_Delay,Available_Seats,SEAT")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flight).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flight);
        }

        // GET: Flights/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flight flight = db.Flights.Find(id);
            if (flight == null)
            {
                return HttpNotFound();
            }
            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Flight flight = db.Flights.Find(id);
            db.Flights.Remove(flight);
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
