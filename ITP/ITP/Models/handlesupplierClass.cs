using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ITP.Models
{
    public class handlesupplierClass
    {
        [Key]
        public int SupplierIndex { get; set; }

        [Required(ErrorMessage ="Enter Supplier Name")]
        [Display(Name = "Name of Supplier")]
         public String NameofSupplier { get; set; }

        [Required(ErrorMessage = "Enter Company Name")]
        [Display(Name = "Company Name")]
        public String Company { get; set; }

        [Required(ErrorMessage = "Enter Email Address")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Required(ErrorMessage = "Enter Phone Number")]
        [Display(Name = "Phone Number")]
        public int PhoneNumber { get; set; }
    }
}
