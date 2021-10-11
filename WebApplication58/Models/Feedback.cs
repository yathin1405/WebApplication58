using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication58.Models
{
    public class Feedback
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? Answer { get; set; }
        [StringLength(500)]

        public string Comment { get; set; }
        [StringLength(100)]

        public string FullName { get; set; }
        [StringLength(255)]

        public string Email { get; set; }
    }
}
