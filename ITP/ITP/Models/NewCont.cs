using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Models
{
    public class NewCont
    {
        [Key]
        public int Cid { get; set; }

        //FirstName
        [Required(ErrorMessage = "Enter First Name")]
        [Display(Name = "Fisrt Name")]
        public string Fname { get; set; }

        //LastName
        [Required(ErrorMessage = "Enter Last Name")]
        [Display(Name = "Last Name")]
        public string Lname { get; set; }

        //Email
        [Required(ErrorMessage = "Enter Email address")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //Phonenumber
        [Required(ErrorMessage = "Enter Phone Number")]
        [Display(Name = "Phone Number")]
        public string Phonen { get; set; }

        //Message
        [Required(ErrorMessage = "Enter Message")]
        [Display(Name = "Message")]
        [DataType(DataType.MultilineText)]
        public string Cmessage { get; set; }
    }
}
