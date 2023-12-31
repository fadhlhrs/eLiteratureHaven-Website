﻿using System;
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
        public int id { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = " • Username is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Maximum 20 characters and minimum 4 characters.")]
        public string username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = " • Password is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Maximum 20 characters and minimum 6 characters.")]
        public string password { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = " • Please enter your Email address")]
        [EmailAddress(ErrorMessage = " • Invalid Email Address")]
        public string email { get; set; }

        [Display(Name = "Role")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string role { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = " • Please enter your full name")]
        public string full_name { get; set; }

        [Display(Name = "Phone Number")]
        public string phone_number { get; set; }

        [NotMapped]
        [Display(Name = "Confirm Password")]
        [Compare("password", ErrorMessage = " • Password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}