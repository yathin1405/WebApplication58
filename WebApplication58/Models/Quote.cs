using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication58.Models
{
    public class Quote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Phone Number is Required")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter Number of Adults")]
        [Display(Name = "Number of Adults")]
        public int NumAdults { get; set; }

        [Required(ErrorMessage = "Please enter Number of Kids")]
        [Display(Name = "Number of Kids")]
        public int NumKids { get; set; }

        [Required(ErrorMessage = "What is your Departure Date?")]
        [Display(Name = "Departure Date")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "What is your Return Date?")]
        [Display(Name = "Return Date")]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Tour Destinations:")]
        public TourList TourL { get; set; }
        public enum TourList
        {
            None,
            Ushaka,
            Kruger_National_Park,
            DurbanNaturalScienceMuseum,
            Cape_of_Good_Hope,
            GoldReefCityThemePark,
            Apartheid_Museum
        }

        [Display(Name = "Cruise Destinations:")]
        public CruiseList CruiseL { get; set; }
        public enum CruiseList
        {
            None,
            MSC,
            Princess,
            Costa,
            Royal

        }

        [Display(Name = "Flight Destinations:")]
        public FlightList FlightL { get; set; }
        public enum FlightList
        {
            None,
            Mango,
            BritishAirways,
            SAA
        }

        [Display(Name = "Hotel Destinations:")]
        public HotelList HotelL { get; set; }
        public enum HotelList
        {
            None,
            City_Lodge,
            Sun_City_Lodge,
            Protea_Hotel,
            Royal_Palm
        }

        [Display(Name = "Estimated Price")]
        public double estimatedPrice { get; set; }
    }
}