using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Models
{
    public class ViewCartDeliveryDetails
    {
        public int Id { get; set; }
        
        public string OrderNo { get; set; }
        [Required(ErrorMessage = "Enter Your or Reciver Name")]
        [Display(Name = "Your or Reciver Name")]
        public string ReceiverName { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "Enter Your Contact Number")]
        [Display(Name = "Contact Number")]
        public string PhoneNo { get; set; }
        [Required]
        [Display(Name = "Phone No")]
        public string City { get; set; }

        [Required(ErrorMessage = "Enter Your Email")]
        [Display(Name = "Your Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public List<ViewCartDetailsModel> viewCartDetailsModelList { get; set; }
        
        //public DateTime OrderDate { get; set; }
    }
}
