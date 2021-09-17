using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Models
{
    public class AttendanceModel
    {
        [Key]

        [Required(ErrorMessage = "Enter Employee ID")]
        [Display(Name = "Employee ID")]
        public int Empid { get; set; }

        [Required(ErrorMessage = "Enter Employee Name")]
        [Display(Name = "Employee Name")]
        public String Empname { get; set; }

        [Required(ErrorMessage = "Enter Date and Time")]
        [Display(Name = "Date and Time")]
        public DateTime EDate { get; set; }

        [Display(Name = "Present")]
        public bool IsAvailable { get; set; }
    }
}
