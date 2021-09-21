using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;


namespace ITP.Models.DriverModels
{
    public class NewDriverClass
    {

        [Key]

        public int Driver_ID { get; set; }


        [Required(ErrorMessage ="Enter Driver Name")]
        [Display(Name ="Drivers Name")]

        public string Driver_Name { get; set; }


        [Required(ErrorMessage = "Enter User Name")]
        [Display(Name = "User Name")]

        public string username { get; set; }


        [Required(ErrorMessage = "Please enter Password")]
        [Display(Name = "Enter Password")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Enter valid password<br>with minimum 8 characters<br>Upper case letters<br>and Special characters")]
        [DataType(DataType.Password)]

        public string Password { get; set; }


        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        [Display(Name = "Re-Enter Password")]
        [DataType(DataType.Password)]

        public string RePassword { get; set; }


        [Required(ErrorMessage = "Enter Driver Description")]
        [Display(Name = "Driver Description")]
        [DataType(DataType.MultilineText)]
        public string Driver_Description { get; set; }


        [Required(ErrorMessage = "Enter Driver Email")]
        [Display(Name = "Driver Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Enter Driver Contact Number")]
        [Display(Name = "Contact Number")]
        [DataType(DataType.PhoneNumber)]
        public string Contact_Number { get; set; }



        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string Driver_Image { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }



        //[Required(ErrorMessage = "Upload your profile picture")]
        //[Display(Name = "Vehicle Type")]
        //public string Driver_Image { get; set; }



        [Required(ErrorMessage = "Enter Vehicle Type")]
        [Display(Name = "Vehicle Type")]

        public string Vehicle_Type { get; set; }



        [Required(ErrorMessage = "Plaese Enter Vehicle Number")]
        [Display(Name = "Vehicle Number")]

        public string Vehicle_Number { get; set; }




        [Required(ErrorMessage = "Enter Vehicle Maximum Capacity")]
        [Display(Name = "Max Capacity")]

        public string Vehicle_Capacity { get; set; }

       

    }
}
