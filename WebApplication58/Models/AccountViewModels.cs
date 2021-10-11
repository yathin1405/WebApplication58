using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication58.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]

        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 35, ErrorMessage = "First Name must be atleast 2 characters long", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        [StringLength(maximumLength: 35, ErrorMessage = "Last Name must be atleast 2 characters long", MinimumLength = 2)]
        public string LastName { get; set; }



        [Required]
        [Display(Name = "Phone Number")]
        [DataType(dataType: DataType.PhoneNumber)]
        [RegularExpression(pattern: @"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-. ]?([0-9]{8})$", ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(maximumLength: 10, ErrorMessage = "SA Contact Number must be exactly 10 digits long", MinimumLength = 10)]
        public string phone { get; set; }


        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Physical Address Field is required")]

        [DataType(DataType.MultilineText)]
        [MinLength(3, ErrorMessage = "Physical Address must be atleast 3 characters long")]
        [MaxLength(150, ErrorMessage = "Physical Address must not exceed 150 characters")]
        [Display(Name = "Physical Address")]
        public string Address { get; set; }
        [Display(Name = "UserType")]
        public string UserType { get; set; }
        public virtual List<GuestDetails> guest { get; set; }
        public virtual List<Reservation> reservationns { get; set; }

        public virtual List<Flight> Flights { get; set; }
        public virtual List<Cruise> Cruise { get; set; }
        //public virtual List<Reservation> reservationns { get; set; }
        public virtual List<Tour> Tours { get; set; }
        public RegisterViewModel() { }

        public RegisterViewModel(ApplicationUser user)
        {
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.phone = user.PhoneNumber;
            this.Address = user.Address;
            //this.reservationns = user.reservationns;
        }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RegisterAllViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "User Account")]
        [Required(ErrorMessage = "Please Select Account Type")]
        public string AccountType { get; set; }

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


        [Required(ErrorMessage = "Please Enter RSA ID")]
        [Display(Name = "RSA ID")]
        [StringLength(13, ErrorMessage = "ID cannot have more than 13 characters")]
        public string IdentityNumber { get; set; }


        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Please select Gender")]
        public string Gender { get; set; }


        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "Please enter your Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}
