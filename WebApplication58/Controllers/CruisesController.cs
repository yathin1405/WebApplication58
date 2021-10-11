using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication58.Models;

namespace WebApplication58.Controllers
{
    public class CruisesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cruises
        public async Task<ActionResult> Index()
        {
            return View(await db.Cruises.ToListAsync());
        }

        // GET: Cruises/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cruise cruise = await db.Cruises.FindAsync(id);
            if (cruise == null)
            {
                return HttpNotFound();
            }
            return View(cruise);
        }
        public ActionResult Detail()
        {
            string id = User.Identity.Name;
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.Email == id);
            var model = new RegisterViewModel(user);
            model.Cruise.Sort();
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public ActionResult guestDetails()
        {
            List<Cruise> gd = new List<Cruise>();
            var list = (from u in db.Users.Where(x => x.FirstName == HttpContext.User.Identity.Name)
                        join r in db.Cruises on u.Email equals r.Email
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
                            r.DepartureTime,
                            r.Departure_Date,
                            r.Cruise_Duration,
                            r.Num_Guests,
                            r.Ship_Name,


                        }).ToList();
            foreach (var it in list)
            {
                Cruise gdd = new Cruise();
                gdd.Cruise_Duration = it.Cruise_Duration;
                gdd.Num_Guests = it.Num_Guests;
                gdd.Email = it.Email;
                gdd.Departure_Date = it.Departure_Date;
                gdd.DepartureTime = it.DepartureTime;
                gdd.Ship_Name = it.Ship_Name;

                gdd.FROM = it.FROM;
                gdd.TO = it.TO;
                //gdd.BasicCharge = it.BasicCharge;
                //gdd.TotalPrice = it.TotalPrice;
                //gdd.Discount = it.Discount;

                gd.Add(gdd);
            }
            return View(gd);
        }

        // GET: Cruises/Create
        public ActionResult Create(string id)
        {
            var flight = db.Cruises.Find(id);
            Session["CruiseID"] = id;
            ViewBag.Id = new SelectList(db.Users, "Id", "Email");
            ViewBag.cruise = new SelectList(db.Cruises, "Id", "Email");
            return View();
        }

        // POST: Cruises/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CruiseID,ShipName,FROM,TO,Ship_Name,Cruise_Duration,Num_Guests,Departure_Date,DepartureTime")] Cruise cruise)
        {
            if (ModelState.IsValid)
            {
                db.Cruises.Add(cruise);
                await db.SaveChangesAsync();
                Session["bookID"] = cruise.CruiseID;
                return RedirectToAction("ConfirmCruise", new { CruiseID = cruise.CruiseID });
            }

            return View(cruise);
        }
        public ActionResult ConfirmFlight(int? CruiseID)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Flights.Add(flight);
            //    db.SaveChanges();
            //    Session["bookID"] = flight.FlightID;
            //    return RedirectToAction("Payment");
            //}
            {
                if (CruiseID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Cruise reservation = db.Cruises.Find(CruiseID);
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

                var rm = db.Cruises.Find(reservation.CruiseID);
                //var rmb = db.roomTypes.Find(rm.RoomTypeId);


                Cruise Gdetails = new Cruise();
                Gdetails.TO = reservation.TO;
                Gdetails.FROM = reservation.FROM;
                Gdetails.Cruise_Duration = reservation.Cruise_Duration;
                Gdetails.ShipName = reservation.ShipName; ;
                Gdetails.Departure_Date = reservation.Departure_Date;
                Gdetails.DepartureTime = reservation.DepartureTime;
                Gdetails.Num_Guests = reservation.Num_Guests;
                //Gdetails.NumAdults = reservation.NumAdults;
                //Gdetails.FirstName = reservation.FirstName;
                //Gdetails.LastName = reservation.LastName;
                Gdetails.Email = reservation.Email;


                //Gdetails.bookings = 0;
                //Gdetails. = rm.passengerCost();
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

        // GET: Cruises/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cruise cruise = await db.Cruises.FindAsync(id);
            if (cruise == null)
            {
                return HttpNotFound();
            }
            return View(cruise);
        }

        // POST: Cruises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CruiseID,ShipName,FROM,TO,Ship_Name,Cruise_Duration,Num_Guests,Departure_Date,DepartureTime")] Cruise cruise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cruise).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cruise);
        }

        // GET: Cruises/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cruise cruise = await db.Cruises.FindAsync(id);
            if (cruise == null)
            {
                return HttpNotFound();
            }
            return View(cruise);
        }

        // POST: Cruises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Cruise cruise = await db.Cruises.FindAsync(id);
            db.Cruises.Remove(cruise);
            await db.SaveChangesAsync();
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
