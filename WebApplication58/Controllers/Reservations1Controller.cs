using WebApplication58.Models;

using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace WebApplication58.Controllers
{
    [Authorize]
    public class Reservations1Controller : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservations1
        public ActionResult Index(string searchString)
        {
            var courses = from c in db.reservations select c;
            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(c => c.Id == searchString);
            }
            //var userid = User.Identity.GetUserId();
            //var reserves = db.reservations.Where(r => r.Id == userid).Include(r => r.applicationUser).Include(r => r.rooms);

            //var reservations = db.reservations.Include(r => r.applicationUser).Include(r => r.rooms);
            //return View(reserves.ToList());
            return View(courses.Include(r => r.applicationUser).Include(r => r.rooms).OrderByDescending(x => x.BookingID));
        }


        // GET: Reservations1/Details/5
        //public ActionResult Details(int? id)
        //{
        //    //string id = User.Identity.Name;
        //    //var Db = new ApplicationDbContext();
        //    //var user = Db.Users.First(u => u.Email == id);
        //    //var model = new RegisterViewModel(user);
        //    //model.reservations.Sort();
        //    //return View(model);
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Reservation reservation = db.reservations.Find(id);
        //    if (reservation == null)
        //    {-
        //        return HttpNotFound();
        //    }
        //    return View(reservation);
        //}

        public ActionResult Detail()
        {
            string id = User.Identity.Name;
            var Db = new ApplicationDbContext();
            var user = Db.Users.First(u => u.Email == id);
            var model = new RegisterViewModel(user);
            model.reservationns.Sort();
            return View(model);
        }
        [HttpGet]
        [Authorize]
        public ActionResult guestDetails()
        {
            List<GuestDetails> gd = new List<GuestDetails>();
            var list = (from u in db.Users.Where(x => x.UserName == HttpContext.User.Identity.Name)
                        join r in db.reservations on u.Id equals r.Id
                        join rm in db.Rooms on r.RoomNo equals rm.RoomNo
                        join rt in db.roomTypes on rm.RoomTypeId equals rt.RoomTypeId
                        select new
                        {
                            u.FirstName,
                            u.LastName,
                            u.Email,
                            u.PhoneNumber,
                            r.checkInDate,
                            r.checkOutDate,
                            r.numGuests,
                            r.DayNo,
                            rm.RoomNo,
                            rm.FloorNum,
                            rt.Type,
                            rt.BasicCharge,
                            r.TotalPrice,
                            r.Discount
                        }).ToList();
            foreach (var it in list)
            {
                GuestDetails gdd = new GuestDetails();
                gdd.FirstName = it.FirstName;
                gdd.LastName = it.LastName;
                gdd.Email = it.Email;
                gdd.PhoneNumber = it.PhoneNumber;
                gdd.checkInDate = it.checkInDate;
                gdd.checkOutDate = it.checkOutDate;
                gdd.numGuests = it.numGuests;
                gdd.DayNo = it.DayNo;
                gdd.RoomNo = it.RoomNo;
                gdd.FloorNum = it.FloorNum;
                gdd.Type = it.Type;
                gdd.BasicCharge = it.BasicCharge;
                gdd.TotalPrice = it.TotalPrice;
                gdd.Discount = it.Discount;

                gd.Add(gdd);
            }
            return View(gd);
        }

        // GET: Reservations1/Create
        public ActionResult Create(string id)
        {
            var room = db.Rooms.Find(id);
            Session["roomId"] = id;
            ViewBag.Id = new SelectList(db.Users, "Id", "Email");
            ViewBag.roomNo = new SelectList(db.Rooms, "RoomNo", "RoomNo");

            return View();
        }

        // POST: Reservations1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookingID,checkInDate,checkOutDate,numGuests,Id,RoomNo,DateBooked,checkedIn,checkedOut,date_CheckedOut,shuttle,DayNo,bookings,TotalPrice,Paymentstatus,AdvancedPayment")] Reservation reservation)
        {

            var BookCount = db.reservations.ToList().Where(x => x.applicationUser == User.Identity).Count();
            reservation.RoomNo = Session["roomId"].ToString();
            var myroom = db.Rooms.Find(reservation.RoomNo);
            ////var days = db.Rooms.Find(reservation.RoomNo);
            //var rtpe = db.roomTypes.Find(myroom.RoomTypeId);

            //var cost =rtpe.BasicCharge*  reservation.DayNo;           

            myroom.NumOfRooms = myroom.NumOfRooms - 1;
            myroom.NoBooked = myroom.NoBooked + 1;

            db.Entry(myroom).State = EntityState.Modified;
            db.SaveChanges();


            var userId = User.Identity.Name;
            var MemberMail = db.Users.ToList().Find(x => x.Email == HttpContext.User.Identity.Name).Id;
            if (MemberMail == null)
            {
                ModelState.AddModelError("", "You must be logged in to make a booking");
            }

            var userid = User.Identity.GetUserId();
            reservation.Id = userid;

            reservation.DateBooked = DateTime.Now;
            reservation.DayNo = Convert.ToInt32(reservation.CalcDaysStay());

            if (ModelState.IsValid)
            {



                myroom.NumOfRooms = 1;
                if (reservation.checkInDate >= reservation.checkOutDate)
                {
                    TempData["res"] = "Check in Date can't come after Check Out Date";
                    ViewBag.Id = new SelectList(db.Users, "Id", "Email", reservation.Id);
                    ViewBag.RoomNo = new SelectList(db.Rooms, "RoomNo", "RoomNo", reservation.RoomNo);
                    return View(reservation);
                }
                if (myroom.NumOfRooms == 0)
                {
                    myroom.RoomStatus = "Fully Booked";
                }

                ApplicationUser userr = new ApplicationUser();

                if (reservation.getbookings() > 3)
                {
                    reservation.Discount = (reservation.DayNo * myroom.roomPrice() * 0.1);
                    reservation.TotalPrice = reservation.DayNo * myroom.roomPrice() - (reservation.DayNo * myroom.roomPrice() * 0.1);
                }

                else
                {
                    reservation.TotalPrice = reservation.DayNo * myroom.roomPrice();
                    reservation.Discount = (reservation.DayNo * myroom.roomPrice() * 0.1);

                }

                if (reservation.checkedIn == true)
                {
                    reservation.status = "Reserved";
                    myroom.RoomStatus = "Occupied";
                    db.SaveChanges();
                }
                else if (reservation.checkedOut == true)
                {
                    reservation.status = "Checked Out";
                    myroom.RoomStatus = "Available";
                }
                reservation.Id = MemberMail;
                reservation.countZ(userid);
                reservation.bookings = reservation.countZ(userid);
                var client = db.Users.Find(userid);
                client.numBookings = client.numBookings + 1;
                reservation.Discount = reservation.CalcBasicPrice() * 0.1;

                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();

                if (reservation.roomChecker(reservation.checkInDate) == true)
                {
                    if (reservation.CheckRoomGuests(reservation.numGuests) == true)
                    {
                        db.reservations.Add(reservation);
                        db.SaveChanges();
                        Session["bookID"] = reservation.BookingID;

                        try
                        {
                            //var userId = User.Identity.Name;
                            //var MemberMail = db.Users.ToList().Find(x => x.Email == HttpContext.User.Identity.Name).Id;
                            var myMessage = new SendGridMessage
                            {
                                From = new EmailAddress("no-reply@homify.co.za", "No-Reply")
                            };


                            myMessage.AddTo("whatichris@gmail.com"/*User.Identity.Name*/);
                            string subject = "Booking Successful";
                            string body = ("Dear " + User.Identity.Name + "<b/r>" +
                                 " " + "We would like to inform you that your Booking at Olwandle Hotel is successful please see below the details of your Booking." + "<b/r>" + " " + " Date : " + reservation.DateBooked + "<b/r>" + "Room Number: " + " " + reservation.RoomNo + "<b/r>" + " " + "Number of Guests: " + " " + reservation.numGuests + "<b/r>" + " " + "Total Price: " + " " + reservation.TotalPrice);

                            //
                            myMessage.Subject = subject;
                            myMessage.HtmlContent = body;




                            var transportWeb = new SendGrid.SendGridClient("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");

                            await transportWeb.SendEmailAsync(myMessage);
                        }
                        catch (Exception x)
                        {
                            Console.WriteLine(x);
                        }
                        //var apiKey = Environment.GetEnvironmentVariable("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");
                        //var Client = new SendGridClient(apiKey);
                        //var from = new EmailAddress("ntmakam0@gmail.com.com", "Nzama");
                        //var subject = "Sending with SendGrid is Fun";
                        //var to = new EmailAddress("ntmakamo@gmail.com", "Chris");
                        //var plainTextContent = "and easy to do anywhere, even with C#";
                        //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
                        //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                        //var response = await Client.SendEmailAsync(msg);

                        return RedirectToAction("ConfirmReservation", new { BookingID = reservation.BookingID });
                        //return RedirectToAction("Create", "Pays");
                    }
                    else
                    {
                        if (reservation.CheckRoomGuests(reservation.numGuests) == false)
                        {
                            ModelState.AddModelError("", "The room is cannot contain more than " + reservation.numGuests);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The room is already reserved please book for another one or book a date later than " + reservation.outDate());
                }

                //db.reservations.Add(reservation);
                //db.SaveChanges();
                //Session["bookID"] = reservation.BookingID;
                //return RedirectToAction("ConfirmReservation", new { BookingID = reservation.BookingID });


            }

            ViewBag.Id = new SelectList(db.Users, "Id", "Email", reservation.Id);
            ViewBag.RoomNo = new SelectList(db.Rooms, "RoomNo", "RoomNo", reservation.RoomNo);
            return View(reservation);
        }
        public ActionResult PayFast()
        {
            //string email = User.Identity.Name;
            int ID = int.Parse(Session["bookID"].ToString());

            Reservation res = new Reservation();

            res = db.reservations.Find(ID);
            res.TotalPrice = res.TotalPrice;
            try
            {
                // Create the order in your DB and get the ID
                string amount = res.TotalPrice.ToString();
                string orderId = new Random().Next(1, 9999).ToString();
                //string orderId = res.applicationUser.FirstName + "  " + res.applicationUser.LastName; 
                string name = "SeaView , Booking Order #" + orderId;
                string description = "Payments For Rooms";

                string site = "";
                string merchant_id = "";
                string merchant_key = "";

                // Check if we are using the test or live system
                string paymentMode = System.Configuration.ConfigurationManager.AppSettings["PaymentMode"];

                if (paymentMode == "test")
                {
                    site = "https://sandbox.payfast.co.za/eng/process?";
                    merchant_id = "10010464";
                    merchant_key = "r8y2nuhvkd3kd";
                }
                else if (paymentMode == "live")
                {
                    site = "https://www.payfast.co.za/eng/process?";
                    merchant_id = System.Configuration.ConfigurationManager.AppSettings["PF_MerchantID"];
                    merchant_key = System.Configuration.ConfigurationManager.AppSettings["PF_MerchantKey"];
                }
                else
                {
                    throw new InvalidOperationException("Cannot process payment if PaymentMode (in web.config) value is unknown.");
                }

                // Build the query string for payment site

                StringBuilder str = new StringBuilder();
                str.Append("merchant_id=" + HttpUtility.UrlEncode(merchant_id));
                str.Append("&merchant_key=" + HttpUtility.UrlEncode(merchant_key));
                str.Append("&return_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_ReturnURL"]));
                str.Append("&cancel_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_CancelURL"]));
                str.Append("&notify_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_NotifyURL"]));

                str.Append("&m_payment_id=" + HttpUtility.UrlEncode(orderId));
                str.Append("&amount=" + HttpUtility.UrlEncode(amount));
                str.Append("&item_name=" + HttpUtility.UrlEncode(name));
                str.Append("&item_description=" + HttpUtility.UrlEncode(description));

                // Redirect to PayFast
                TempData["payment"] = "Payment for Booking was successful";
                Response.Redirect(site + str.ToString());
                // RedirectToAction("guestDetails", "Reservations1");
            }
            catch (Exception ex)
            {
                // Handle your errors here (log them and tell the user that there was an error)
            }
            return View();
        }
        public ActionResult SPayFast()
        {
            var userId = User.Identity.Name;

            var email = User.Identity.Name;
            //var FindEmail = dc.Users.Where(m => m.UserName == email);
            //var Del = db.DeliveryOptions.Count(m => m.UserName == email);
            //double delivery = 0;
            //if (Del > 0)
            //{
            //    delivery = 100;
            //}
            //string EmailFound = "";
            //foreach (var item in FindEmail)
            //{
            //    EmailFound = item.Email;
            //}

            Reservation rn = new Reservation();
            double amount = rn.TotalPrice;
            string orderId = new Random().Next(1, 99999).ToString();
            string name = "Blissful Kiddies, Order#" + orderId;
            string description = "Blissful Kiddies";


            string site = "";
            string merchant_id = "";
            string merchant_key = "";

            // Check if we are using the mmor live system
            string paymentMode = System.Configuration.ConfigurationManager.AppSettings["PaymentMode"];

            if (paymentMode == "test")
            {
                site = "https://sandbox.payfast.co.za/eng/process?";
                merchant_id = "10000100";
                merchant_key = "46f0cd694581a";
            }
            else if (paymentMode == "live")
            {
                site = "https://www.payfast.co.za/eng/process?";
                merchant_id = System.Configuration.ConfigurationManager.AppSettings["PF_MerchantID"];
                merchant_key = System.Configuration.ConfigurationManager.AppSettings["PF_MerchantKey"];
            }
            else
            {
                throw new InvalidOperationException("Cannot process payment if PaymentMode (in web.config) value is unknown.");
            }
            // Build the query string for payment site

            StringBuilder str = new StringBuilder();
            str.Append("merchant_id=" + HttpUtility.UrlEncode(merchant_id));
            str.Append("&merchant_key=" + HttpUtility.UrlEncode(merchant_key));
            str.Append("&return_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_ReturnURL"]));
            str.Append("&cancel_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_CancelURL"]));
            //str.Append("&notify_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_NotifyURL"]));

            str.Append("&m_payment_id=" + HttpUtility.UrlEncode(orderId));
            str.Append("&amount=" + HttpUtility.UrlEncode(amount.ToString()));
            str.Append("&item_name=" + HttpUtility.UrlEncode(name));
            str.Append("&item_description=" + HttpUtility.UrlEncode(description));

            // Redirect to PayFast
            Response.Redirect(site + str.ToString());

            return View();
        }

        public ActionResult ConfirmReservation(int? BookingID)
        {
            if (BookingID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.reservations.Find(BookingID);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            Rooms rooms = db.Rooms.Where(x => x.RoomNo == reservation.RoomNo).FirstOrDefault();

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

            var rm = db.Rooms.Find(reservation.RoomNo);
            var rmb = db.roomTypes.Find(rm.RoomTypeId);


            GuestDetails Gdetails = new GuestDetails();
            Gdetails.checkInDate = reservation.checkInDate;
            Gdetails.checkOutDate = reservation.checkOutDate;
            Gdetails.numGuests = reservation.numGuests;
            Gdetails.DayNo = Convert.ToInt32(reservation.CalcDaysStay());
            //Gdetails.bookings = 0;
            Gdetails.RoomNo = rm.RoomNo;
            Gdetails.FloorNum = rm.FloorNum;
            Gdetails.Type = rooms.Roomtype.Type;
            Gdetails.BasicCharge = rmb.BasicCharge;
            Gdetails.TotalPrice = reservation.TotalPrice;
            Gdetails.Discount = reservation.Discount;
            return View(Gdetails);
        }
        public ActionResult Bill(int? BookingID)
        {
            if (BookingID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.reservations.Find(BookingID);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            Rooms rooms = db.Rooms.Where(x => x.RoomNo == reservation.RoomNo).FirstOrDefault();

            var rm = db.Rooms.Find(reservation.RoomNo);
            var rmb = db.roomTypes.Find(rm.RoomTypeId);

            AddPrice ap = new AddPrice();

            Bill bill = new Bill();
            bill.checkInDate = reservation.checkInDate;
            bill.checkOutDate = reservation.checkOutDate;
            bill.DayNo = Convert.ToInt32(reservation.CalcDaysStay());
            bill.RoomNo = rm.RoomNo;
            bill.BasicCharge = rmb.BasicCharge;
            bill.BreakFast = bill.BreakFast;
            bill.Lunch = bill.Lunch;
            bill.Supper = bill.Supper;
            bill.BasicCharge = rmb.BasicCharge;
            bill.BPrice = 100;
            bill.LPrice = 120;
            bill.DPrice = 140;
            bill.TotalAmount = ((100 + 120 + 140) + rmb.BasicCharge) - reservation.Discount;
            bill.Discount = reservation.Discount;
            return View(bill);
        }
        public ActionResult Confirm(int? BookingID)
        {
            if (BookingID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.reservations.Find(BookingID);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            Rooms rooms = db.Rooms.Where(x => x.RoomNo == reservation.RoomNo).FirstOrDefault();

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

            var rm = db.Rooms.Find(reservation.RoomNo);
            var rmb = db.roomTypes.Find(rm.RoomTypeId);

            GuestDetails Gdetails = new GuestDetails();
            Gdetails.checkInDate = reservation.checkInDate;
            Gdetails.checkOutDate = reservation.checkOutDate;
            Gdetails.numGuests = reservation.numGuests;
            Gdetails.DayNo = Convert.ToInt32(reservation.CalcDaysStay());
            //Gdetails.bookings = 0;
            Gdetails.RoomNo = rm.RoomNo;
            Gdetails.FloorNum = rm.FloorNum;
            Gdetails.Type = rooms.Roomtype.Type;
            Gdetails.BasicCharge = rmb.BasicCharge;
            Gdetails.TotalPrice = reservation.TotalPrice;

            return View(Gdetails);
        }



        // GET: Reservations1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "Email", reservation.Id);
            ViewBag.RoomNo = new SelectList(db.Rooms, "RoomNo", "RoomNo", reservation.RoomNo);
            return View(reservation);
        }

        // POST: Reservations1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingID,checkInDate,checkOutDate,numGuests,Id,RoomNo,DateBooked,checkedIn,checkedOut,date_CheckedOut,shuttle,DayNo,bookings,TotalPrice")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {

                reservation.checkOutDate = reservation.checkOutDate;
                if (reservation.checkedIn == true)
                {
                    reservation.status = "Checked In";
                    db.Entry(reservation).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                if (reservation.checkedOut == true && reservation.checkedIn == true)
                {
                    var r = db.Rooms.ToList().Where(x => x.RoomNo == reservation.RoomNo).Select(p => p.RoomTypeId).FirstOrDefault();
                    Rooms rooms = db.Rooms.Where(x => x.RoomNo == reservation.RoomNo).FirstOrDefault();
                    rooms.NumOfRooms = rooms.NumOfRooms + 1;
                    reservation.status = "Checked Out";
                    db.Entry(reservation).State = EntityState.Modified;
                    db.SaveChanges();
                    reservation.date_CheckedOut = DateTime.Now;
                    return RedirectToAction("Bill", new { BookingID = reservation.BookingID });
                }
                else
                {
                    ModelState.AddModelError("", "You cannot Check Out the guest without checking them in");
                }

                //db.Entry(reservation).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }
            Bill bill = new Bill();
            ViewBag.Id = new SelectList(db.Users, "Id", "Email", reservation.Id);
            ViewBag.RoomNo = new SelectList(db.Rooms, "RoomNo", "RoomNo", reservation.RoomNo);
            return View(reservation);
        }


        // GET: Reservations1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reservation reservation = db.reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            if (reservation.checkedOut == true)
            {
                ModelState.AddModelError("", "You cannot cancel this booking because the guest has already CheckedOut ");
                //return RedirectToAction("");
            }
            return View(reservation);
        }

        // POST: Reservations1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.reservations.Find(id);
            Cancellation c = new Cancellation();
            c.checkInDate = reservation.checkInDate;
            c.checkOutDate = reservation.checkOutDate;
            c.DateBooked = reservation.DateBooked;
            c.date_CheckedOut = reservation.date_CheckedOut;
            c.DayNo = reservation.DayNo;
            c.numGuests = reservation.numGuests;
            c.TotalPrice = reservation.TotalPrice;
            c.RoomNo = reservation.RoomNo;
            reservation.status = "Cancelled";
            //if (reservation.checkedOut == true)
            //{
            //    ModelState.AddModelError("", "You cannot cancel this booking because the guest has already CheckedOut ");
            //    return RedirectToAction("");
            //}
            db.Cancellations.Add(c);
            db.reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index", "Cancellations");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult AvailableRooms(string id)
        {
            var rooms = db.Rooms.Include(r => r.Roomtype).Where(x => x.RoomStatus == "Vacant");
            return View(rooms.ToList());
        }

        public string ConfirmBooking(int? BookingID)
        {
            Reservation reservation = db.reservations.Find(BookingID);
            reservation.BookingStatus = "Confirmed";
            db.SaveChanges();
            return ("Reservation has been confirmed");
        }
        public ActionResult ViewStatus()
        {
            string m = HttpContext.User.Identity.Name;
            List<Reservation> ReservationList = db.reservations.ToList().FindAll(p => p.Id == m);

            foreach (var item in ReservationList)
            {
                if (item.BookingStatus == "Confirmed")
                {
                    ViewBag.c = "Confirmed";
                }
                else if (item.BookingStatus == null)
                {
                    ViewBag.n = "Not Yet Confirmed";
                }

            }
            return View(ReservationList);
        }
        public ActionResult ConfirmedBooking(int? id)
        {
            Reservation reservation = db.reservations.Find(id);
            string search = "";
            search = ConfirmBooking(id);
            ViewBag.Search = search;
            return View(reservation);
        }


    }
}
