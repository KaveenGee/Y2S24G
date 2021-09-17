using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ITP.Models
{
    public class excessfaultyclass
    {
        [Key]
        public int SupplierID { get; set; }
        [Required(ErrorMessage = "Enter Product ID")]
        [Display(Name = "Product ID")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Enter Supply Date")]
        [Display(Name = "Supply Date")]
        public DateTime SupplyDate { get; set; }
        
        [Required(ErrorMessage = "Enter Status")]
        [Display(Name = "Enter Status")]
        public String ExcessFaultyStatus { get; set; }

        [Required(ErrorMessage = "Quantity Purchased")]
        [Display(Name = "Quantity Purchased")]
        public int QuantityPurchased { get; set; }

        [Required(ErrorMessage = "Reason")]
        [Display(Name = "Reason")]
        public String Reason { get; set; }


    }
}
