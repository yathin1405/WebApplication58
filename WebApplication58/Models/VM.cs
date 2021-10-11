using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace WebApplication58.Models
{
    public class VM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int VMK { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }


        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public int NumGuests { get; set; }
        public int NumDays { get; set; }
        public double TotalAmount { get; set; }

        public string RoomNo { get; set; }
        public string Type { get; set; }
    }
}
