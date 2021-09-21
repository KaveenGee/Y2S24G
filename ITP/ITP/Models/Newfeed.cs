using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Models
{
    public class Newfeed
    {
        [Key]
        public int FeedID { get; set; }

        //FirstName
        [Required(ErrorMessage = "Enter First Name")]
        [Display(Name = "Fisrt Name")]
        public string FirstName { get; set; }

        //LastName
        [Required(ErrorMessage = "Enter Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //Email
        [Required(ErrorMessage = "Enter Feedback Type")]
        [Display(Name = "Feedback Type")]
        public string FeedType { get; set; }

        //Phonenumber
        [Required(ErrorMessage = "Enter Feedback Comment")]
        [Display(Name = "Feedback Comment")]
        public string FeedDes { get; set; }

        [Display(Name = "Please rate quality of the products:")]
        public int St_01 { get; set; }

        [Display(Name = "Please rate quality of customer service:")]
        public int St_02 { get; set; }

        [Display(Name = "Please rate overall experience with us:")]
        public int Rate { get; set; }
        
    }
}
