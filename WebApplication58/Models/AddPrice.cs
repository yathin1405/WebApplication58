using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication58.Models
{
    public class AddPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int priceID { get; set; }
        [DisplayName("Breakfast Price")]
        public double BPrice { get; set; }
        [DisplayName("Lunch Price")]
        public double LPrice { get; set; }
        [DisplayName("Dinner Price")]
        public double DPrice { get; set; }

    }
}