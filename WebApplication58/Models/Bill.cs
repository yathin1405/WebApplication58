using WebApplication58.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication58.Models
{
    public class Bill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int billID { get; set; }
        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Arrival Date")]
        public DateTime checkInDate { get; set; }
        [Required]
        [Display(Name = "Departure Date")]
        public DateTime checkOutDate { get; set; }
        [Required]
        [DisplayName("No Of Nights")]
        public int DayNo { get; set; }
        [Required]
        [DisplayName("Room No")]
        public string RoomNo { get; set; }
        [Required, Range(0, 25000)]
        [Display(Name = "Room Price")]
        [DisplayFormat(DataFormatString = "{0: 0.00}")]
        public double BasicCharge { get; set; }
        public int BookingId { get; set; }
        public double Discount { get; set; }
        public double TotalAmount { get; set; }
        public bool BreakFast { get; set; }
        public bool Lunch { get; set; }
        public bool Supper { get; set; }
        public double BPrice { get; set; }
        public double LPrice { get; set; }
        public double DPrice { get; set; }


        public virtual Rooms rooms { get; set; }
        public virtual Reservation reservation { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();
        public double getPrice()
        {
            var don = (from p in db.Rooms
                       where p.RoomNo == RoomNo
                       select p.TotalPrice).FirstOrDefault();
            return don;
        }
        public double CalcExtras()
        {
            double cost = 0;
            if (BreakFast == true)
            {
                cost = 100;
            }
            if (Lunch == true)
            {
                cost += 120;
            }
            if (Supper == true)
            {
                cost += 140;
            }
            return cost;
        }
        public double CalcTotalAmountDue()
        {
            return getPrice() + CalcExtras() - getDisc();
        }

        public double getDisc()
        {
            var don = (from d in db.reservations
                       where d.BookingID == BookingId
                       select d.Discount).FirstOrDefault();
            return don;
        }
    }
}