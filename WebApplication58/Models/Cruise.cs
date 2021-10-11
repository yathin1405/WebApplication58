using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication58.Models
{
    public class Cruise
    {

        [Display(Name = "Ship Name")]
        public Ships ShipName { get; set; }
        public enum Ships
        {
            Princess_Cruises,
            Costa_Cruises,
            MSC_Cruises,
            Royal_Caribbean_Cruises,

        }
        [Display(Name = "Email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "From")]
        public From3 FROM { get; set; }
        public enum From3
        {
            Port_Elizabeth,
            Durban,
            Capetown,

        }
        [Display(Name = "To")]
        public To3 TO { get; set; }
        public enum To3
        {
            Port_Elizabeth,
            Durban,
            Capetown,

        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name = "Cruise ID")]
        public int CruiseID { get; set; }



        [Display(Name = "Ship Name")]
        public string Ship_Name { get; set; }

        [Display(Name = "Cruise Duration")]
        public string Cruise_Duration { get; set; }

        [Display(Name = "Number of passengers")]
        [Range(0, 100)]
        public string Num_Guests { get; set; }

        public const float price = 15000;

        //[Display(Name = "From")]
        //public string LocationFrom { get; set; }
        //[Display(Name = "TO")]
        //public string LocationTO { get; set; }

        [Display(Name = "Departure Date")]
        [DataType(DataType.Date)]
        public DateTime Departure_Date { get; set; }


        [Display(Name = "Departure Time")]

        public string DepartureTime { get; set; }

    }
}