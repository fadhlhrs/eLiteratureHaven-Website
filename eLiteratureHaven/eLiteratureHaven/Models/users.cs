using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eLiteratureHaven.Models
{
    public class users
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = " • Username is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Maximum 20 characters and minimum 4 characters.")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = " • Password is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Maximum 20 characters and minimum 6 characters.")]
        public string Password { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = " • Email is required.")]
        [EmailAddress(ErrorMessage = " • Invalid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Role { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = " • Full name is required.")]
        public string Full_name { get; set; }

        [Display(Name = "Date of birth")]
        [Required(ErrorMessage = " • Date of birth is required.")]
        public DateTime Date_of_birth { get; set; }

        [Display(Name = "Phone Number (optional)")]
        public string Phone_number { get; set; }

        [NotMapped]
        [Display(Name = "Confirm Password")]
        [Compare("password", ErrorMessage = " • Password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}