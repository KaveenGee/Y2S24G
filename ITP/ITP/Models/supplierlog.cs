using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ITP.Models
{

    public class supplierlog
    {
        [Key]
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "Enter Product ID ")]
        [Display(Name = "Product ID")]

        public int ProductID { get; set; }

        [Required(ErrorMessage = "Enter ProductCatergory")]
        [Display(Name = "ProductCatergory")]
        public String ProductCatergory { get; set; }

        [Required(ErrorMessage = "Enter Model")]
        [Display(Name = "Model")]
        public String Model { get; set; }

        [Required(ErrorMessage = "Enter Brand Name")]
        [Display(Name = "Brand Name")]
        public String BrandName { get; set; }

        [Required(ErrorMessage = "Enter Supply Date")]
        [Display(Name = "Supply Date")]
        public DateTime SupplyDate { get; set; }

        [Required(ErrorMessage = "Enter Quantity Purchased")]
        [Display(Name = "Quantity Purchased")]
        public int QuantityPurchased { get; set; }

        [Required(ErrorMessage = "Enter Unit Price")]
        [Display(Name = "Unit Price")]
        public String UnitPrice { get; set; }

        [Required(ErrorMessage = "Enter Total Price")]
        [Display(Name = "Total Price")]
        public String TotalPrice { get; set; }

        [Required(ErrorMessage = "Enter Supply Status")]
        [Display(Name = "Supply Status ")]
        public String SupplyStatus { get; set; }


    }

}   
