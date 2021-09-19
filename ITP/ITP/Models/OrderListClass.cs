using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ITP.Models
{
    public class OrderListClass
    {
        [Key]
        public int Deliveryid { get; set; }

        [Required(ErrorMessage = "Enter Order_ID")]
        [Display(Name = "Order_ID")]
        public int Order_ID { get; set; }

        [Required(ErrorMessage ="Enter Receiver name")]
        [Display(Name ="Receiver Name")]
        public String Receivername { get; set; }

        [Required(ErrorMessage ="Enter DeliveryAddress")]
        [Display(Name ="DeliveryAddress")]
        public String DeliveryAddress { get; set; }

        [Required(ErrorMessage ="Enter Phone")]
        [Display(Name ="Phone")]
        public int Phone { get; set; }

        [Required(ErrorMessage = "Enter City")]
        [Display(Name = "City")]
        public String City { get; set; }

        [Required(ErrorMessage ="Enter ODateTime")]
        [Display(Name ="ODateTime")]
        public DateTime ODateTime { get; set; }

        [Required(ErrorMessage ="Enter Driver")]
        [Display(Name ="Driver")]
        public String Driver { get; set; }

        [Required(ErrorMessage ="Enter OStatus")]
        [Display(Name ="OStatus")]
        public String OStatus { get; set; }
    }
}
