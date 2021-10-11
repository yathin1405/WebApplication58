using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using WebApplication58.Models;

namespace WebApplication58.Models
{

    public class WalkinReserve
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResId { get; set; }

        [Required]
        [Display(Name = "Arrival Date")]
        [DataType(DataType.Date)]
        public DateTime CheckinDate { get; set; }
        [Required]
        [Display(Name = "Departure Date")]
        [DataType(DataType.Date)]
        public DateTime CheckoutDate { get; set; }
        public int NumGuests { get; set; }
        public int NumDays { get; set; }
        public double TotalAmount { get; set; }

        public string RoomNo { get; set; }
        public virtual Rooms rooms { get; set; }
        public int RoomTypeId { get; set; }
        public virtual RoomTypes roomtype { get; set; }

        public int ID { get; set; }
        public virtual PersonalData personalDetails { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();
        public double roomPrice()
        {
            var ok = (from rt in db.roomTypes
                      where rt.RoomTypeId == rt.RoomTypeId
                      select rt.BasicCharge).FirstOrDefault();
            return ok;
        }

        public double CalcDaysStay()
        {
            TimeSpan difference = CheckoutDate - CheckinDate;
            var days = difference.TotalDays;
            return days;
        }
        public string getType()
        {
            var don = (from rt in db.roomTypes
                       where RoomTypeId == RoomTypeId
                       select rt.Type).FirstOrDefault();
            return don;
        }

        public double CalcBasicPrice()
        {
            double price = 0;
            if (getType() == "Standard")
            {
                price = 500 * CalcDaysStay();
            }
            else if (getType() == "Family")
            {
                price = 1200 * CalcDaysStay();
            }
            else if (getType() == "Deluxe")
            {
                price = 400 * CalcDaysStay();
            }
            else if (getType() == "Luxury")
            {
                price = 700 * CalcDaysStay();
            }
            else if (getType() == "SeaView")
            {
                price = 900 * CalcDaysStay();
            }
            return price;
        }




    }
}

