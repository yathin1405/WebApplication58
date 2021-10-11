using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace WebApplication58.Models
{
    public class RoomTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomTypeId { get; set; }
        [Required]
        [DisplayName("Room Type")]
        [MinLength(6)]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string Type { get; set; }
        [AllowHtml]
        [Required]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Rooms Available")]
        [Range(1, 300)]
        public int Qty { get; set; }
        public int NumOfRooms { get; set; }

        [Required, Range(0, 25000)]
        [Display(Name = "Room Price")]
        [DisplayFormat(DataFormatString = "{0: 0.00}")]
        public double BasicCharge { get; set; }



        // public int NoBooked { get; set; }

        //ApplicationDbContext db = new ApplicationDbContext();
        //public double getnobooked()
        //{
        //    var book = (from r in db.Rooms
        //                where r.RoomTypeId == RoomTypeId
        //                select r.NoBooked).FirstOrDefault();
        //    return book;
        //}
    }
}