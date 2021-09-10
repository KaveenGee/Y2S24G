using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ITP.Models
{
    public class Customer
    {
        [Key]

        public int Cusid { get; set; }

        [Required(ErrorMessage = "Please Enter Your LastName")]
        [Display(Name = "Customer LastName")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Your FirstName")]
        [Display(Name = "Customer FirstName")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email")]
        [Display(Name = "Customer Email")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Required(ErrorMessage = "Please Enter a Username")]
        [Display(Name = "Username")]
        public String Username { get; set; }

        [Required(ErrorMessage = "Please Enter a Password")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Enter valid password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repeat Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public String ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Your Address")]
        [Display(Name = "Address")]
        public String Address { get; set; }

        public String Image { get; set; }

        [RegularExpression(@"^.*[0-9]{9}[V-v]$", ErrorMessage = "Invalid")]
        [Required(ErrorMessage = "Enter  NIC")]
        public String NIC { get; set; }

        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Enter Phone number")]
        public String PhoneNumber { get; set; }

        [NotMapped]
        [Display(Name = "Upload file")]
        public IFormFile Imagefile { get; set; }

        public DateTime Joindate { get; set; }
    }
}
