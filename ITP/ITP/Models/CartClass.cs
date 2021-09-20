using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Models
{
    public class CartClass
    {
        [Key]
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        public int OrderNo { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public string IBrand { get; set; }
        [Required]
       
        public string IModel { get; set; }
        public int Quntity { get; set; }
        
        public int Price { get; set; }
        public int TotalPrice { get; set; }

        [Required]
        public string PhoneNo { get; set; }

        [Required]        
        public string ReceiverName { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        
        public string ImageName { get; set; }
        
    }
}
