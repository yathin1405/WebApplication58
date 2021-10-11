using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using WebApplication58.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication58.Models
{
    public class Customer

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }


        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your First Name")]
        [StringLength(35, ErrorMessage = "First Name cannot exceed more than 35 characters")]
        [RegularExpression(@"^[A-Z]+[a-z]*$", ErrorMessage = "First Name cannot have special character,numbers or space")]
        public string FName { get; set; }



        [Display(Name = "Middle Name")]
        [RegularExpression(@"^[A-Z]+[a-z]*$", ErrorMessage = "Middle Name cannot have special character,numbers or space")]
        [StringLength(35, ErrorMessage = "Middle Name cannot have more than 35 characters")]
        public string MName { get; set; }



        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Please enter your Surname")]
        [RegularExpression(@"^[A-Z]+[a-z]*$", ErrorMessage = "Surname cannot have special character,numbers or space")]
        [StringLength(35, ErrorMessage = "Surname cannot have more than 35 characters")]
        public string LName { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Please select a title")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Please Enter RSA ID")]
        [Display(Name = "RSA ID")]
        [StringLength(13, ErrorMessage = "ID cannot have more than 13 characters")]
        public string IdentityNumber { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Please select Gender")]
        public string Gender { get; set; }


        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }



    }



    public class Staff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StaffId { get; set; }


        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your First Name")]
        [StringLength(35, ErrorMessage = "First Name cannot exceed more than 35 characters")]
        [RegularExpression(@"^[A-Z]+[a-z]*$", ErrorMessage = "First Name cannot have special character,numbers or space")]
        public string FName { get; set; }



        [Display(Name = "Middle Name")]
        [RegularExpression(@"^[A-Z]+[a-z]*$", ErrorMessage = "Middle Name cannot have special character,numbers or space")]
        [StringLength(35, ErrorMessage = "Middle Name cannot have more than 35 characters")]
        public string MName { get; set; }



        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Please enter your Surname")]
        [RegularExpression(@"^[A-Z]+[a-z]*$", ErrorMessage = "Surname cannot have special character,numbers or space")]
        [StringLength(35, ErrorMessage = "Surname cannot have more than 35 characters")]
        public string LName { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Please select a title")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Please Enter RSA ID")]
        [Display(Name = "RSA ID")]
        [StringLength(13, ErrorMessage = "ID cannot have more than 13 characters")]
        public string IdentityNumber { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Please select Gender")]
        public string Gender { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string FullName()
        {
            return FName + " " + MName + " " + LName;
        }

    }

}








