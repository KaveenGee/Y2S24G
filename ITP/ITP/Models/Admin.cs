using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITP.Models
{
    public class Admin
    {
        [Key]
        public int Admin_ID { get; set; }

        [Required(ErrorMessage = "Please Enter Username")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please password")]
        [Display(Name = "Password")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Re Enter Your Password")]
        [Display(Name = "Re-Enter Password")]
        public string Re_Password { get; set; }

        [Required(ErrorMessage = "Please Enter Your Name")]
        [Display(Name = "Manager Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Your Phone Number")]
        [Display(Name = "Phone Number")]
        public string Phone_Number { get; set; }
        public string Image { get; set; }

        [Required(ErrorMessage = "Please Enter Your NIC")]
        [Display(Name = "NIC")]
        public string NIC { get; set; }

        [Required(ErrorMessage = "Please Enter Qualifications")]
        [Display(Name = "Qualifications")]
        public string Qualifications { get; set; }

        [Required(ErrorMessage = "Please select the type")]
        [Display(Name = "Position")]
        public string Type { get; set; }

        [NotMapped]
        [Display(Name = "Upload file")]
        public IFormFile Imagefile { get; set; }

    }
}
