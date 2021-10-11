using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication58.Models
{
    public class Cancellation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CancelID { get; set; }
        [Display(Name = "Arrival Date")]
        public DateTime checkInDate { get; set; }
        [Display(Name = "Departure Date")]
        public DateTime checkOutDate { get; set; }
        [DisplayName("No of Guests")]
        [Range(1, 6, ErrorMessage = "Please book for another room because this room cannot occupy more then 6 people")]
        public int numGuests { get; set; }
        public string Email { get; set; }

        [DisplayName("Room No.")]
        public string RoomNo { get; set; }
        [DisplayName("Date Booked")]
        public DateTime DateBooked { get; set; }

        [Display(Name = "Checked In?")]
        public bool checkedIn { get; set; }

        [Display(Name = "Checked Out?")]
        public bool checkedOut { get; set; }
        public Nullable<DateTime> date_CheckedOut { get; set; }
        [DisplayName("No Of Nights")]
        public int DayNo { get; set; }

        [DisplayName("Number of bookings")]
        public int bookings { get; set; } = 0;

        [DisplayName("Total Price")]
        public double TotalPrice { get; set; }
        public double Discount { get; set; }
        public string Status { get; set; }
    }
}