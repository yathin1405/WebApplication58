using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication58.Models
{
    public class ViewModel
    {
        [Key]
        public int view { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Display(Name = "First Name")]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 35, ErrorMessage = "First Name must be atleast 2 characters long", MinimumLength = 2)]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 35, ErrorMessage = "Last Name must be atleast 2 characters long", MinimumLength = 2)]
        public string LastName { get; set; }


        [Display(Name = "Arrival Date")]

        public DateTime checkInDate { get; set; }

        [Display(Name = "Departure Date")]
        public DateTime checkOutDate { get; set; }

        [Display(Name = "No of Guests")]
        [Range(1, 6, ErrorMessage = "Please book for another room because this room cannot occupy more then 6 people")]
        public int numGuests { get; set; }


        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }
    }
}