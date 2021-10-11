using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication58.Models;

namespace WebApplication58.Models
{
    public class Shuttle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PickupId { get; set; }
        public string Id { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser applicationUser { get; set; }
        [Required]
        [DisplayName("Source")]
        public string source { get; set; }
        [Required]
        [DisplayName("Shuttle Price")]
        public double finacharge { get; set; }
        public bool pickedup { get; set; }

        //public List<Payments> Payments { get; set; }
    }



}
