using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication58.Models;

namespace WebApplication58.Models
{
    public class Rooms
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string RoomNo { get; set; }

        [Required]
        [DisplayName("Room Type ID")]
        public int RoomTypeId { get; set; }
        public RoomTypes Roomtype { get; set; }

        [Required]
        [DisplayName("Floor Number")]
        public int FloorNum { get; set; }

        [Display(Name = "Picture")]
        public byte[] Picture { get; set; }
        public string Id { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser applicationUser { get; set; }

        [DisplayName("Room Status")]
        public string RoomStatus { get; set; }

        [Required]
        public int NoBooked { get; set; }
        [Required]
        [DisplayName("Total Price")]
        public double TotalPrice { get; set; }
        [DisplayName("Number of Rooms")]
        public int NumOfRooms { get; set; }
        [DisplayName("Number of Occupents :")]
        public int numguests { get; set; }
        [DisplayName("Number of beds :")]

        public int Numbeds { get; set; }

        //public int BookingID { get; set; }
        //public Reservation reservation { get; set; }
        public string getType()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var type = (from t in db.roomTypes
                        where t.RoomTypeId == RoomTypeId
                        select t.Type).FirstOrDefault();
            return type;
        }
        public double price()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var pr = (from t in db.roomTypes
                      where t.RoomTypeId == RoomTypeId
                      select t.BasicCharge).FirstOrDefault();
            return pr;
        }
        public int numRoom()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var num = (from t in db.roomTypes
                       where t.RoomTypeId == RoomTypeId
                       select t.Qty).FirstOrDefault();
            return num;
        }

        public int CountNumRooms()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Rooms.ToList().Count;
        }
        //public int countRooms()
        //{

        //}

        //public string RoomNumber()
        //{
        //    string record = "";
        //    if ((CountNumRooms() >= 0) && (CountNumRooms() <= 10) && (getType() == "Standard"))
        //    {
        //        record = "RM" + CountNumRooms();
        //        FloorNum = 1;
        //    }
        //    if ((CountNumRooms() > 10) && (CountNumRooms() <= 20) && (getType() == "Family"))
        //    {
        //        record = "RM" + CountNumRooms();
        //        FloorNum = 2;
        //    }
        //    if ((CountNumRooms() > 20) && (CountNumRooms() <= 30) && (getType() == "Delux"))
        //    {
        //        record = "RM" + CountNumRooms();
        //        FloorNum = 3;
        //    }
        //    if ((CountNumRooms() > 30) && (CountNumRooms() <= 40) && (getType() == "Luxury"))
        //    {
        //        record = "RM" + CountNumRooms();
        //        FloorNum = 4;
        //    }
        //    if ((CountNumRooms() > 40) && (CountNumRooms() <= 50) && (getType() == "SeaView"))
        //    {
        //        record = "RM" + CountNumRooms();
        //        FloorNum = 5;
        //    }

        //    return record;
        //}
        public int NoDays()
        {
            var days = db.reservations.ToList().OrderByDescending(x => x.RoomNo == RoomNo).Select(x => x.DayNo).FirstOrDefault();
            //var days = (from d in db.reservations
            //            where d.RoomNo==RoomNo
            //            select d.DayNo).OrderByDescending();
            return days;
        }
        public double roomPrice()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var num = (from t in db.roomTypes
                       where t.RoomTypeId == RoomTypeId
                       select t.BasicCharge).FirstOrDefault();
            return num;
        }
        public int getbookings()
        {
            var bookings = (from u in db.Users
                            where u.Id == Id
                            select u.numBookings).FirstOrDefault();
            return bookings;
        }

        public double CalcTotalPrice()
        {
            //double price = 0;

            //if (getType() == "Standard")
            //{

            //    price = (roomPrice() * NoDays());

            //}
            //else if (getType() == "Family")
            //{


            //    price = roomPrice() * NoDays();

            //}
            //else if (getType() == "Deluxe")
            //{

            //    price = roomPrice() * NoDays();

            //}
            //else if (getType() == "Luxury")
            //{

            //    price = roomPrice() * NoDays();

            //}
            //else if (getType() == "SeaView")
            //{

            //    price = roomPrice() * NoDays();

            //}
            return roomPrice() * NoDays();
        }

        public double discount()
        {
            double cost = 0;
            if (getbookings() >= 3 && getbookings() <= 5)
            {
                cost = CalcTotalPrice() * 0.10;
            }
            else if (getbookings() > 5)
            {
                cost = CalcTotalPrice() * 0.15;
            }
            else
            {
                cost = 0;
            }
            return cost;
        }
        public double CalcFinalCharge()
        {
            return CalcTotalPrice() + discount();
        }
        public string generateRoomNumber()
        {
            Random rasn = new Random();
            NumOfRooms = 0;
            string ok = "";
            if (getType() == "Standard")
            {
                int num = rasn.Next(1, 10);
                ok = "SRN" + num;
                FloorNum = 1;
                NumOfRooms++;
            }
            else if (getType() == "Family")
            {
                int num = rasn.Next(11, 20);
                ok = "FRN" + num;
                FloorNum = 2;
                NumOfRooms++;
            }
            else if (getType() == "Deluxe")
            {
                int num = rasn.Next(21, 30);
                ok = "DRN" + num;
                FloorNum = 3;
                NumOfRooms++;
            }
            else if (getType() == "Luxury")
            {
                int num = rasn.Next(31, 40);
                ok = "LRN" + num;
                FloorNum = 4;
                NumOfRooms++;
            }
            else if (getType() == "SeaView")
            {
                int num = rasn.Next(41, 50);
                ok = "SVRN" + num;
                FloorNum = 5;
                NumOfRooms++;
            }

            return ok;

        }

        ApplicationDbContext db = new ApplicationDbContext();

        public string ChangeQty()
        {
            var verdict = "";


            var r = db.roomTypes.ToList().Where(x => x.RoomTypeId == RoomTypeId).Select(p => p.RoomTypeId).FirstOrDefault();


            var rt = db.roomTypes.Find(r);
            if (NumOfRooms == 0)
            {
                verdict = "Rooms Exceeded";
                return verdict;
            }
            else
                NumOfRooms -= 1;
            db.SaveChanges();
            verdict = "Available";
            return verdict;
        }

        public int ChangeNoBooked()
        {
            int book = 0;

            var rId = db.Rooms.ToList().Where(x => x.RoomNo == RoomNo).Select(p => p.RoomNo).FirstOrDefault();

            var RNo = db.Rooms.Find(rId);
            book = RNo.NoBooked += 1;
            db.SaveChanges();
            return book;
        }
    }
}