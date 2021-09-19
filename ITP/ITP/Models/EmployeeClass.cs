using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ITP.Models
{
    public class EmployeeClass
    {
        [Key]
        public int Empid { get; set; }


       

        [Required(ErrorMessage = "Enter Empname")]
        [Display(Name = "Empname")]

        public string Empname { get; set; }

        [Required(ErrorMessage = "Enter EmpNIC")]
        [Display(Name = "EmpNIC")]

        public int EmpNIC { get; set; }

        [Required(ErrorMessage = "Enter EmpAddress")]
        [Display(Name = "EmpAddress")]

       
        public string EmpAddress { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Enter EmpEmail")]
        [Display(Name = "EmpEmail")]

        public String EmpEmail { get; set; }
        [Required(ErrorMessage = "Enter EmpGender")]
        [Display(Name = "EmpGender")]

        public String EmpGender { get; set; }
        [Required(ErrorMessage = "Enter EmpStatus")]
        [Display(Name = "EmpStatus")]

        public String EmpStatus { get; set; }
    
    [Required(ErrorMessage = "Enter EmpContactNo")]
        [Display(Name = "EmpContactNo")]

        public int EmpContactNo { get; set; }

        [Required(ErrorMessage = "Enter EmpDateofApproval")]
        [Display(Name = "EmpDateofApproval")]

        public int EmpDateofApproval { get; set; }

        [Required(ErrorMessage = "Enter EmpAge")]
        [Display(Name = "EmpAge")]

        public int EmpAge { get; set; }
        [Required(ErrorMessage =  "Enter EmpSalary")]
        [Display(Name = "EmpSalary")]

        public int EmpSalary { get; set; }
}
}
