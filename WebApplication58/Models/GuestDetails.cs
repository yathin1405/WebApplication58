using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication58.Models
{
    public class GuestDetails
    {
        [Key]
        public int g { get; set; }
        [Required]
        [Display(Name = "Arrival Date")]
        public DateTime checkInDate { get; set; }
        [Required]
        [Display(Name = "Departure Date")]
        public DateTime checkOutDate { get; set; }
        [Required]
        [DisplayName("No Of Nights")]
        public int DayNo { get; set; }
        [DisplayName("No of Guests")]
        //[Range(1, 6, ErrorMessage = "Please book for another room because this room cannot occupy more then 6 people")]
        public int numGuests { get; set; }
        [Required]
        [DisplayName("Room No")]
        public string RoomNo { get; set; }
        [Required]
        [DisplayName("Floor Number")]
        public int FloorNum { get; set; }
        [Required]
        [DisplayName("Room Type")]
        [MinLength(6)]
        public string Type { get; set; }
        [Required, Range(0, 25000)]
        [Display(Name = "Room Price")]
        [DisplayFormat(DataFormatString = "{0: 0.00}")]
        public double BasicCharge { get; set; }
        public int BookingID { get; set; }
        public Reservation reserve { get; set; }

        [Required]
        [DisplayName("Total Price")]
        public double TotalPrice { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public double Discount { get; set; }



        //public GuestDetails() { }

        //public GuestDetails(ApplicationUser user)
        //{
        //    this.checkInDate = reservation.checkInDate;
        //    this.checkOutDate = reservation.checkOutDate;
        //    this.DayNo = reservation.DayNo;
        //    this.BookingID = reservation.BookingID;
        //    this.numGuests = reservation.numGuests;
        //    this.TotalPrice = reservation.TotalPrice;


        //}
    }
}