using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Models
{
    public class ItemModel
    {
        [Key]

        [Required(ErrorMessage = "Enter Item ID")]
        [Display(Name = "Item ID")]
        public int IItemId { get; set; }

        [Required(ErrorMessage = "Enter Date and Time")]
        [Display(Name = "Date and Time")]
        public DateTime IDateTime { get; set; }

        [Required(ErrorMessage = "Enter Brand")]
        [Column(TypeName = "nvarchar(150)")]
        [Display(Name = "Brand")]
        public String IBrand { get; set; }

        [Required(ErrorMessage = "Enter Item Model")]
        [Column(TypeName = "nvarchar(150)")]
        [Display(Name = "Model")]
        public String IModel { get; set; }

        [Required(ErrorMessage = "Enter Item Category")]
        [Column(TypeName = "nvarchar(150)")]
        [Display(Name = "Category")]
        public String ICategory { get; set; }

        [Required(ErrorMessage = "Enter Quantity")]
        [Display(Name = "Quantity Purchase")]
        public int IQPurchase { get; set; }

        [Required(ErrorMessage = "Enter Quantity in Stock")]
        [Display(Name = "Quantity in Stock")]
        public int IQStock { get; set; }

        [Required(ErrorMessage = "Enter Quantity Sold")]
        [Display(Name = "Quantity Sold")]
        public int IQSold { get; set; }

        [Required(ErrorMessage = "Enter Unit Price")]
        [Display(Name = "Unit Price")]
        public decimal IUPrice { get; set; }

        [Required(ErrorMessage = "Enter Total Price")]
        [Display(Name = "Total Price")]
        public decimal ITPrice { get; set; }

        [Required(ErrorMessage = "Enter IInventory Value")]
        [Display(Name = "Inventory Value")]
        public decimal IIValue { get; set; }

        [Required(ErrorMessage = "Enter Discount")]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Discount")]
        public String IDiscount { get; set; }

        [Required(ErrorMessage = "Enter Description")]
        [Column(TypeName = "nvarchar(500)")]
        [Display(Name = "Description")]
        public String IDescription { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
