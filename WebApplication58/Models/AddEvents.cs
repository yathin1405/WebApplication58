using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication58.Models
{
    public class AddEvents
    {
        [Key]
        public string EventId { get; set; }
        [Required]
        [Display(Name = "Event Type")]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string EventType { get; set; }
        [Required]
        [Display(Name = "Maximum Number Of Guests")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public int MaxNumAttNum { get; set; }
        [Required]
        [Display(Name = "Day Event Price")]
        [Range(1, double.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public double DEventP { get; set; }
        [Required]
        [Display(Name = "Night Event Price")]
        [Range(1, double.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public double NEventP { get; set; }
        [Required]
        [Display(Name = "Night First Discount")]
        [Range(1, double.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public double NightLessD { get; set; }
        [Required]
        [Display(Name = "Day Second Discount")]
        [Range(1, double.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public double DayGreaterD { get; set; }
        [Display(Name = "Day First Discount")]

        public double DayLessD { get; set; }
        [Required]
        [Display(Name = "Night Second Discount")]
        [Range(1, double.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]

        public double NightGreaterD { get; set; }
        [Required]
        [Display(Name = "Min Discount Qual")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]

        public int MDQA { get; set; }

    }
}