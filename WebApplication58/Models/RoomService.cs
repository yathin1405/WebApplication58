using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication58.Models
{
    public class RoomService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int servicesID { get; set; }
        public bool BreakFast { get; set; }
        public bool Launch { get; set; }
        public bool Dinner { get; set; }

        public int priceID { get; set; }
        public virtual AddPrice price { get; set; }
    }
}