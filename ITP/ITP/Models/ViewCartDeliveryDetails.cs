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
      
        public string ReceiverName { get; set; }
       
        public string DeliveryAddress { get; set; }
      
        public string PhoneNo { get; set; }
      
        public string City { get; set; }
      
        public string Email { get; set; }

        public List<ViewCartDetailsModel> viewCartDetailsModelList { get; set; }
        
        //public DateTime OrderDate { get; set; }
    }
}
