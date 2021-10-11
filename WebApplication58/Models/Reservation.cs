using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.ModelBinding;
using WebApplication58.Models;

namespace WebApplication58.Models
{
    public class Reservation : IComparable<Reservation>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingID { get; set; }
        [Required]
        [Display(Name = "Arrival Date")]
        [DataType(DataType.Date)]
        public DateTime checkInDate { get; set; }
        [Required]
        [Display(Name = "Departure Date")]
        [DataType(DataType.Date)]
        public DateTime checkOutDate { get; set; }
        [Required]
        [DisplayName("No of Guests")]
        //[Range(1, 6, ErrorMessage = "Please book for another room because this room cannot occupy more then 6 people")]
        public int numGuests { get; set; }
        public string Id { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser applicationUser { get; set; }

        [DisplayName("Room No.")]
        public string RoomNo { get; set; }
        public Rooms rooms { get; set; }
        [DisplayName("Date Booked")]
        public DateTime DateBooked { get; set; }

        [Display(Name = "Checked In?")]
        public bool checkedIn { get; set; }

        [Display(Name = "Checked Out?")]
        public bool checkedOut { get; set; }
        public Nullable<DateTime> date_CheckedOut { get; set; }


        [Required]
        [DisplayName("No Of Nights")]
        public int DayNo { get; set; }
        [Required]
        [DisplayName("Number of bookings")]
        public int bookings { get; set; } = 0;
        [Required]
        [DisplayName("Total Price")]
        public double TotalPrice { get; set; }
        public double Discount { get; set; }
        public string status { get; set; }
        public string Paymentstatus { get; set; }

        public double AdvancedPayment { get; set; }
        //public int RoomTypeId { get; set; }
        //public RoomTypes Roomtype { get; set; }
        //public List<Payments> Payments { get; set; }


        public string BookingStatus { get; set; }
        public string BookingStatusX = "Not yet Confirmed";




        public bool roomChecker(DateTime arrivalDate)
        {
            bool check = false;
            var outDate = (from r in db.reservations
                           where r.RoomNo == RoomNo
                           select r.checkOutDate
                         ).FirstOrDefault();
            if (arrivalDate >= outDate)
            {
                check = true;
            }
            return check;
        }



        public DateTime outDate()
        {
            var outDate = (from r in db.reservations
                           where r.RoomNo == RoomNo
                           select r.checkOutDate
                         ).FirstOrDefault();
            return outDate;
        }

        public bool CheckRoomGuests(int guests)
        {
            bool check = false;

            var guest = (
                from g in db.Rooms
                where g.RoomNo == RoomNo
                select g.numguests
                ).FirstOrDefault();

            if (guests <= guest)
            {
                check = true;
            }

            return (check);
        }



        public int CompareTo(Reservation other)
        {
            return this.checkInDate.Date.Add(this.checkOutDate.TimeOfDay).CompareTo(other.checkInDate.Add(other.checkOutDate.TimeOfDay));

        }

        public bool checkRom()
        {
            var roomst = (from rom in db.Rooms
                          where rom.RoomNo == RoomNo
                          select rom.RoomStatus).FirstOrDefault();
            bool result = false;
            if (roomst == "Available")
            {
                result = true;
            }
            return result;
        }
        public int countZ(string id)
        {
            var countUs = (from res in db.reservations
                           where res.Id == id
                           select res.BookingID).Count();
            return countUs;
        }
        public double CalcDaysStay()
        {
            TimeSpan difference = checkOutDate - checkInDate;
            var days = difference.TotalDays;
            return days;
        }
        ApplicationDbContext db = new ApplicationDbContext();
        //private object reservation;

        public int DecreaseQty()
        {

            ApplicationDbContext db = new ApplicationDbContext();
            RoomTypes ry = new RoomTypes();
            var qty = db.roomTypes.ToList().Find(x => x.RoomTypeId == ry.RoomTypeId).Qty - 1;
            db.Entry(ry).State = EntityState.Modified;
            db.SaveChanges();
            return qty;
        }
        public string roomtype()
        {
            var ok = (from rt in db.roomTypes
                      where rt.RoomTypeId == rt.RoomTypeId
                      select rt.Type).FirstOrDefault();
            return ok;
        }
        //public int getbookings()
        //{
        //    var bookings = (from u in db.Users
        //                    where u.Id == Id
        //                    select u.numBookings).FirstOrDefault();
        //    return bookings;
        //}

        public double roomPrice()
        {
            var ok = (from rt in db.roomTypes
                      where rt.RoomTypeId == rt.RoomTypeId
                      select rt.BasicCharge).FirstOrDefault();
            return ok;
        }
        //public double ExtraPrice()
        //{
        //    var ex = (from e in db.Extras
        //              where e.BookingID == BookingID
        //              select e.BasicCost).FirstOrDefault();
        //    return ex;
        //}

        //public bool gym ()
        //{
        //    var ex = (from e in db.Extras
        //              where e.BookingID == BookingID
        //              select e.Gym).FirstOrDefault();
        //    return ex;
        //}
        //public bool laundry()
        //{
        //    var ex = (from e in db.Extras
        //              where e.BookingID == BookingID
        //              select e.Laundry).FirstOrDefault();
        //    return ex;
        //}
        //public bool parking()
        //{
        //    var ex = (from e in db.Extras
        //              where e.BookingID == BookingID
        //              select e.Parking).FirstOrDefault();
        //    return ex;
        //}

        //public double calcExtras()
        //{
        //    double cost=0;
        //    if (parking() == true)
        //    {
        //        cost = 75 * DayNo;
        //    }
        //    if (gym() == true)
        //    {
        //        cost += 20 * DayNo;
        //    }
        //    if (laundry() == true)
        //    {
        //        cost += 25;
        //    }
        //    return cost;
        //}
        public DateTime getCheckinDate()
        {
            var don = (from d in db.reservations
                       where d.RoomNo == RoomNo
                       select d.checkInDate).FirstOrDefault();
            return don;
        }


        public double CalcBasicPrice()
        {
            double price = 0;
            if (roomtype() == "Standard")
            {
                price = 500 * DayNo;
            }
            else if (roomtype() == "Family")
            {
                price = 1200 * DayNo;
            }
            else if (roomtype() == "Deluxe")
            {
                price = 400 * DayNo;
            }
            else if (roomtype() == "Luxury")
            {
                price = 700 * DayNo;
            }
            else if (roomtype() == "SeaView")
            {
                price = 900 * DayNo;
            }
            return price;
        }


        public int getbookings()
        {
            var bookings = (from u in db.Users
                            join i in db.reservations
                            on u.Id equals i.Id
                            select u.numBookings).FirstOrDefault();
            return bookings;
        }

        public double discount()
        {
            double cost = 0;
            if (getbookings() >= 3 && getbookings() <= 5)
            {
                cost = CalcBasicPrice() * 0.10;
            }
            else if (getbookings() > 5)
            {
                cost = CalcBasicPrice() * 0.15;
            }
            else
            {
                cost = 0;
            }
            return cost;
            //}
            //public double SubTotal()
            //{
            //    return (CalcBasicPrice() - discount()) + calcExtras();
            //}      

            //public double CalcDeposit()
            //{
            //    return (SubTotal() * (10 / 100));
            //}
            //public double CalcOutstanding()
            //{
            //    return SubTotal() - CalcDeposit();
            //}
        }
    }
}
//public string noGuests()
//{
//    string guest = "";
//    if (numGuests > 3 && roomtype()=="Standard")
//    {
//        guest = "Please book for another room, because this room cannot contain more than 3 people";
//    }
//     if (numGuests > 4 && roomtype() == "Deluxe")
//    {
//        guest = "Please book for another room, because this room cannot contain more than 4 people";
//    }
//    if (numGuests > 6 && roomtype() == "Family")
//    {
//        guest = "Please book for another room, because this room cannot contain more than 6 people";
//    }
//    if (numGuests  > 2 && roomtype() == "Luxury")
//    {
//        guest = "Please book for another room, because this room cannot contain more than 2 people";
//    }
//    if (numGuests > 1 && roomtype() == "Family")
//    {
//        guest = "Please book for another room, because this room cannot contain more than 1 person";
//    }
//    return guest;
//}

//public int DecreaseQty()
//{

//    ApplicationDbContext db = new ApplicationDbContext();
//    RoomTypes ry = new RoomTypes();
//    var qty = db.roomTypes.ToList().Find(x => x.RoomTypeId ==ry.RoomTypeId).Qty - 1;
//    db.Entry(ry).State = EntityState.Modified;
//    db.SaveChanges();
//    return qty;
//}